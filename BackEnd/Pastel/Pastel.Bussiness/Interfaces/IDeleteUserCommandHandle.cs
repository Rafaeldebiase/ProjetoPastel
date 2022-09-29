using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IDeleteUserCommandHandle
    {
        Task<ResultDto> Delete(DeleteUserCommand command);
    }
}
