using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.CommandHandle
{
    public class AutenticateCommandHandle : IAutenticateCommandHandle
    {
        private readonly ILogger<AutenticateCommandHandle> _logger;
        private readonly IUserRepository _repository;

        public AutenticateCommandHandle(ILogger<AutenticateCommandHandle> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<ResultUserDto> Autenticate(AutenticateCommand command)
        {
            var result = new ResultUserDto();

            try
            {
                var usersDto = await _repository.GetUserByEmail(command.Email);
                if (usersDto.Count() <= 0)
                {
                    result.AddError("Usuário não encontrado ou senha inválida");
                    return result;
                }

                if (usersDto?.FirstOrDefault()?.Password != command.Password)
                {
                    result.AddError("Usuário não encontrado ou senha inválida");
                    return result;
                }

                var userAuth = UserDto.UserDtoFactory.GenerateToAuth(usersDto?.FirstOrDefault()?.FirstName,
                    usersDto?.FirstOrDefault()?.LastName, usersDto?.FirstOrDefault()?.Role,
                    usersDto?.FirstOrDefault()?.Id);

                result.AddUser(userAuth);
                return result;
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
    }
}
