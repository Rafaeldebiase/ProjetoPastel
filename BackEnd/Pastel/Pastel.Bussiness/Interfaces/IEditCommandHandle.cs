using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IEditTaskCommandHandle
    {
        Task<ResultDto> Edit(EditTaskCommand command);
    }
}
