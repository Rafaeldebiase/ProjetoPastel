using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface ICreateTaskCommandHandle
    {
        Task<ResultDto> Create(CreateTaskCommand command);
    }
}
