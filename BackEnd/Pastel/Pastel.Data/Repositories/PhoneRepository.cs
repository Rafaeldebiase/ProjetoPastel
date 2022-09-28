using Dapper;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Entities;

namespace Pastel.Data.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly DbSession _dbSession;

        public PhoneRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<bool> Ingestion(UserPhone usersPhone)
        {
            var query = $@"
                            insert into pastel.tb_phone
                            (
                                id,
                                user_id,
                                phone_number,
                                phone_type 
                            ) values (
                                '{usersPhone.Id}',
                                '{usersPhone.UserId}',
                                '{usersPhone.Number}',
                                '{usersPhone.Type}'
                            );
                        ";

            var result = await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);
            return result > 0;
        }
    }
}
