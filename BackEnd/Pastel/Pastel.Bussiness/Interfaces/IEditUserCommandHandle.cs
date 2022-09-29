using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IEditUserCommandHandle
    {
        Task<ResultDto> Edit(UserEditCommand command);
    }
}
