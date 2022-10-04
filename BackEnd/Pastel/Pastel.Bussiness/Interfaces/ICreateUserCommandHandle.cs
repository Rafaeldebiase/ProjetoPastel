using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface ICreateUserCommandHandle
    {
        Task<ResultUserDto> Create(CreateUserCommand command);
    }
}
