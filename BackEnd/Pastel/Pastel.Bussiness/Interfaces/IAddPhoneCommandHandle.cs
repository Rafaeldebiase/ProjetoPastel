using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IAddPhoneCommandHandle
    {
        Task<ResultDto> Add(AddPhoneCommand command);
    }
}
