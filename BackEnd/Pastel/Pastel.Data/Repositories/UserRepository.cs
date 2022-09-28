using Dapper;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Entities;
using Pastel.Domain.Enums;

namespace Pastel.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSession _dbSession;

        public UserRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<bool> Save(User user)
        {
            var query = $@"
                            insert into pastel.tb_user
                            (
                                id,
                                first_name,
                                last_name,
                                birth_date,
                                email,
                                pass,
                                street,
                                street_number,
                                street_complement,
                                neighborhood,
                                city,
                                state,
                                country,
                                zip_code,
                                user_role,
                                manager_id
                            ) values (
                                '{user.Id}',
                                '{user.FullName.FirstName}',
                                '{user.FullName.LastName}',
                                '{user.BirthDate.ToString("yyyyMMdd")}',
                                '{user.Email.Address}',
                                '{user.Password.Code}',
                                '{user.Address.Street}',
                                {user.Address.Number},
                                '{(user.Address.Complement is null ? "" : user.Address.Complement)}',
                                '{user.Address.Neighborhood}',
                                '{user.Address.City}',
                                '{user.Address.State}',
                                '{user.Address.Contry}',
                                '{user.Address.ZipCode}',
                                '{Enum.GetName<Role>(user.Role)}',
                                '{user.ManagerId}'
                            );    
                        ";

                var result = await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);
                return result > 0;
        }

        public Task<bool> FindManager(Guid id)
        {
            return Task.FromResult(true);
        }
    }
}
