using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IDeleteTaskCommandHandle
    {
        Task<ResultDto> Delete(DeleteTaskCommand command);
    }
}
