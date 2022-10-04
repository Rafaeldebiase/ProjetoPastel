using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IPhoneHandle
    {
        Task<ResultPhoneDto> GetPhonesByUserId(Guid userId);
    }
}
