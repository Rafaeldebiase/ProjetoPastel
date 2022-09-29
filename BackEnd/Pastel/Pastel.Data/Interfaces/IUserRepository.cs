using Pastel.Domain.Dto;
using Pastel.Domain.Entities;

namespace Pastel.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> FindManager(Guid id);
        Task<bool> FindEmail(string? email);
        Task<IEnumerable<UserDto>> GetUserById(Guid? id);
        Task<IEnumerable<UserDto>> GetUserByEmail(string? email);

        Task<bool> Save(User user);
        Task Edit(User user);
        Task Delete(Guid id);



    }
}
