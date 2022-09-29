using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IAutenticateCommandHandle
    {
        Task<ResultUserDto> Autenticate(AutenticateCommand command);
    }
}
