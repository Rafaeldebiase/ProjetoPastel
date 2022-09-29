using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IRemovePhoneCommandHandle
    {
        Task<ResultDto> Remove(RemovePhoneCommand command);
    }
}
