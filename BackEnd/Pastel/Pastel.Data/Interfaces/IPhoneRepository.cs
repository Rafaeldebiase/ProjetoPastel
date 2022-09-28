using Pastel.Domain.Entities;

namespace Pastel.Data.Interfaces
{
    public interface IPhoneRepository
    {
        Task<bool> Ingestion(UserPhone usersPhone);

    }
}
