using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface ICreateUserCommandHandle
    {
        Task<ResultDto> Create(CreateUserCommand command);
    }
}
