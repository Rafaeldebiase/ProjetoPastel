using Pastel.Domain.Dto;
using Pastel.Domain.Entities;

namespace Pastel.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> FindManager(Guid id);
        Task<IEnumerable<UserDto>> GetUser(Guid id);
        Task<bool> Save(User user);
        Task Edit(User user);
        Task Delete(Guid id);



    }
}
