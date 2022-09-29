using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface ICloseTaskCommandHandle
    {
        Task<ResultDto> Close(CloseTaskCommand command);
    }
}
