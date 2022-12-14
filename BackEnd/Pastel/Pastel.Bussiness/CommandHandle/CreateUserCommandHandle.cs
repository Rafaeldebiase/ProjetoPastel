using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using Pastel.Domain.Enums;
using Pastel.Domain.ValuesObject;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.CommandHandle
{
    public class CreateUserCommandHandle : ICreateUserCommandHandle
    {
        private readonly ILogger<CreateUserCommandHandle> _logger;
        private readonly IUserRepository _repository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandle(ILogger<CreateUserCommandHandle> logger, IUserRepository userRepository,
            IPhoneRepository phoneRepository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repository = userRepository;
            _phoneRepository = phoneRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultUserDto> Create(CreateUserCommand command)
        {
            try
            {
                Enum.TryParse<Role>(command.Role, false, out var role);

                switch (role)
                {
                    case Role.USER:
                        return await CreateUser(command);
                    default:
                        return await CreateDefault(command);
                }
            }
            catch (Exception error)
            {
                var message = $"{error.InnerException}\n " +
                    $"{error.Message} \n " +
                    $"{error.StackTrace}";

                _logger.LogError(message);
                
                var result = new ResultUserDto();
                result.AddError(message);

                return result;
            }
        }

        private async Task<ResultUserDto> CreateUser(CreateUserCommand command)
        {
            var result = new ResultUserDto();

            try
            {
                if (!await FindManager(command.ManagerId))
                {
                    result.AddError("O id do gestor não foi encontrado");
                    return result;
                }

                return await CreateDefault(command);
            }
            catch (Exception error)
            {
                var message = $"{error.InnerException}\n " +
                    $"{error.Message} \n " +
                    $"{error.StackTrace}";

                _logger.LogError(message);

                result.AddError(message);

                return result;
            }
        }

        private async Task<ResultUserDto> CreateDefault(CreateUserCommand command)
        {
            var result = new ResultUserDto();

            try
            {
                var checkEmail = await _repository.FindEmail(command.Email);
                if (checkEmail)
                {
                    result.AddError("Email já cadastrado");
                    return result;
                }

                var user = User.UserFactory.Generate(command);

                _unitOfWork.BeginTransaction();
                await _repository.Save(user);
                await PhoneIngestion(command.Phones, user.Id);
                _unitOfWork.Commit();

                var resultUserDto = UserDto.UserDtoFactory.GenerateFromUser(user);

                result.AddUser(resultUserDto);
                return result;
            }
            catch (Exception error)
            {
                _unitOfWork.Rollback();
                var message = $"{error.InnerException}\n " +
                    $"{error.Message} \n " +
                    $"{error.StackTrace}";

                _logger.LogError(message);

                result.AddError(message);

                return result;
            }
        }

        private async Task<bool> FindManager(string? managerId)
        {
            Guid.TryParse(managerId, out var id);

            return await _repository.FindManager(id);
        }

        private async Task PhoneIngestion(List<Phone>? phones, Guid? userId)
        {
            var usersPhone = UserPhone.PhoneFactory.Create(phones, userId);
            foreach (var phone in usersPhone)
            {
                await _phoneRepository.Ingestion(phone);
            }
        }

    }
}
