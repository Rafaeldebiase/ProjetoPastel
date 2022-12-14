using Pastel.Domain.Dto;
using Pastel.Domain.Entities;

namespace Pastel.Data.Interfaces
{
    public interface IPhoneRepository
    {
        Task<bool> Ingestion(UserPhone usersPhone);
        Task<IEnumerable<PhoneUserDto>> GetPhonesByUserId(Guid userId);
        Task<bool> Remove(UserPhone userPhone);

    }
}
