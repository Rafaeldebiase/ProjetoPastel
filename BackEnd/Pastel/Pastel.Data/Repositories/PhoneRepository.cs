using Dapper;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Dto;
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

        public async Task<IEnumerable<PhoneUserDto>> GetPhonesByUserId(Guid userId)
        {
            var query = @$"
                            select
                            id {nameof(PhoneUserDto.Id)},
                            user_id  {nameof(PhoneUserDto.UserId)},
                            phone_number {nameof(PhoneUserDto.Number)},
                            phone_type {nameof(PhoneUserDto.Type)}
                            from pastel.tb_phone
                            where user_id = '{userId}'
                        ";
            return await _dbSession.Connection.QueryAsync<PhoneUserDto>(query);
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

        public async Task<bool> Remove(UserPhone phone)
        {
            var query = $@"
                            delete from pastel.tb_phone
                            where user_id = '{phone.UserId}'
                            and phone_number = '{phone.Number}'
                            and phone_type = '{phone.Type}'
                        ";

            var result = await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);
            return result > 0;
        }
    }
}
