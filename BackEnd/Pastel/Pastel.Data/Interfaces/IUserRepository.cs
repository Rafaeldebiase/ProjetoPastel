using Pastel.Domain.Entities;

namespace Pastel.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> FindManager(Guid id);

        Task<bool> Save(User user);

    }
}
