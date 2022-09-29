using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using Pastel.Domain.Enums;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.CommandHandle
{
    public class EditUserCommandHandle : IEditUserCommandHandle
    {
        private readonly ILogger<EditUserCommandHandle> _logger;
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EditUserCommandHandle(ILogger<EditUserCommandHandle> logger, IUserRepository repository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultDto> Edit(UserEditCommand command)
        {
            var result = new ResultDto();
            try
            {
                Guid.TryParse(command.Id, out var id);
                var userDto = await _repository.GetUser(id);
                var user = Check(command, userDto);
                _unitOfWork.BeginTransaction();
                await _repository.Edit(user);
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

        private User Check(UserEditCommand command, IEnumerable<UserDto> userDto)
        {
            var user = User.UserFactory.Generate(userDto.FirstOrDefault());
            user = EditFirstName(user, command);
            user = EditLastName(user, command);
            user = EditBirthDate(user, command);
            user = EditEmail(user, command);
            user = EditStreet(user, command);
            user = EditStreetNumber(user, command);
            user = EditStreetComplement(user, command);
            user = EditNeighborhood(user, command);
            user = EditCity(user, command);
            user = EditState(user, command);
            user = EditContry(user, command);
            user = EditRole(user, command);
            user = EditManagerId(user, command);

            return user;
        }

        private User EditFirstName(User user, UserEditCommand command)
        {
            if(command.FirstName != null)
            {
                var fullName = user.FullName.ChangeFirstName(command.FirstName);
                return user.ChangeFullName(fullName);
            }

            return user;
        }

        private User EditLastName(User user, UserEditCommand command)
        {
            if (command.LastName != null)
            {
                var fullName = user.FullName.ChangeLastName(command.LastName);
                return user.ChangeFullName(fullName);
            }

            return user;
        }

        private User EditEmail(User user, UserEditCommand command)
        {
            if (command.Email != null)
            {
                var email = user.Email.ChangeAddress(command.Email);
                return user.ChangeEmail(email);
            }

            return user;
        }

        private User EditBirthDate(User user, UserEditCommand command)
        {
            if (command.BirthDate != null)
            {
                return user.ChangeBirthDate(command.BirthDate.Value);
            }

            return user;
        }

        private User EditStreet(User user, UserEditCommand command)
        {
            if (command.Street != null)
            {
                var address = user.Address.ChangeStreet(command.Street);
                return user.ChangeAddress(address);
            }

            return user;
        }

        private User EditStreetNumber(User user, UserEditCommand command)
        {
            if (command.StreetNumber != null)
            {
                var address = user.Address.ChangeNumber(command.StreetNumber);
                return user.ChangeAddress(address);
            }

            return user;
        }

        private User EditStreetComplement(User user, UserEditCommand command)
        {
            if (command.StreetComplement != null)
            {
                var address = user.Address.ChangeComplement(command.StreetComplement);
                return user.ChangeAddress(address);
            }

            return user;
        }

        private User EditNeighborhood(User user, UserEditCommand command)
        {
            if (command.Neighborhood != null)
            {
                var address = user.Address.ChangeNeighborhood(command.Neighborhood);
                return user.ChangeAddress(address);
            }

            return user;
        }

        private User EditCity(User user, UserEditCommand command)
        {
            if (command.City != null)
            {
                var address = user.Address.ChangeCity(command.City);
                return user.ChangeAddress(address);
            }

            return user;
        }

        private User EditState(User user, UserEditCommand command)
        {
            if (command.State != null)
            {
                var address = user.Address.ChangeState(command.State);
                return user.ChangeAddress(address);
            }

            return user;
        }

        private User EditContry(User user, UserEditCommand command)
        {
            if (command.Contry != null)
            {
                var address = user.Address.ChangeCountry(command.Contry);
                return user.ChangeAddress(address);
            }

            return user;
        }

        private User EditZipCode(User user, UserEditCommand command)
        {
            if (command.ZipCode != null)
            {
                var address = user.Address.ChangeZipCode(command.ZipCode);
                return user.ChangeAddress(address);
            }

            return user;
        }

        private User EditRole(User user, UserEditCommand command)
        {
            if (command.Role != null)
            {
                Enum.TryParse<Role>(command.Role, out var role);
                return user.ChangeRole(role);
            }

            return user;
        }

        private User EditManagerId(User user, UserEditCommand command)
        {
            if (command.ManagerId != null)
            {
                Guid.TryParse(command.ManagerId, out var id);
                return user.ChangeManagerId(id);
            }

            return user;
        }
    }
}

