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
        private readonly IUserRepository _userRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandle(ILogger<CreateUserCommandHandle> logger, IUserRepository userRepository,
            IPhoneRepository phoneRepository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _userRepository = userRepository;
            _phoneRepository = phoneRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultDto> Create(CreateUserCommand command)
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
                
                var result = new ResultDto();
                result.AddError(message);

                return result;
            }
        }

        private async Task<ResultDto> CreateUser(CreateUserCommand command)
        {
            var result = new ResultDto();

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

        private async Task<ResultDto> CreateDefault(CreateUserCommand command)
        {
            var result = new ResultDto();

            try
            {
                var user = GenerateUser(command);

                _unitOfWork.BeginTransaction();
                await _userRepository.Save(user);
                await PhoneIngestion(command.Phones, user.Id);
                _unitOfWork.Commit();

                result.AddObject(user);
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

        private User GenerateUser(CreateUserCommand command)
        {
            var fullName = new FullName(command.FirstName, command.LastName);
            var email = new Email(command.Email);
            var password = new Password(command.Password);
            var address = new Address(
                    command.Street,
                    command.StreetNumber,
                    command.StreetComplement,
                    command.Neighborhood,
                    command.City,
                    command.State,
                    command.Contry,
                    command.ZipCode
                );
            Enum.TryParse<Role>(command.Role, out var role);
            Guid.TryParse(command.ManagerId, out var managerId);

            return new(
                    fullName,
                    command.BirthDate,
                    email,
                    password,
                    address,
                    role,
                    managerId
                );

        }

        private async Task<bool> FindManager(string? managerId)
        {
            Guid.TryParse(managerId, out var id);

            return await _userRepository.FindManager(id);
        }

        private async Task PhoneIngestion(List<Phone>? phones, Guid userId)
        {
            var usersPhone = UserPhone.PhoneFactory.Create(phones, userId);
            foreach (var phone in usersPhone)
            {
                await _phoneRepository.Ingestion(phone);
            }
        }

    }
}
