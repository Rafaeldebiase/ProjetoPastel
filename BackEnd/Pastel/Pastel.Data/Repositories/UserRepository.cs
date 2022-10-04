using Dapper;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Dto;
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

        public async Task<IEnumerable<UserTaskDto>> GetUsers(Guid managerId)
        {
            var query = $@"
                            select
                            u.id {nameof(UserTaskDto.Id)},
                            u.first_name {nameof(UserTaskDto.FirstName)},
                            u.last_name {nameof(UserTaskDto.LastName)},
                            u.birth_date {nameof(UserTaskDto.BirthDate)},
                            u.email {nameof(UserTaskDto.Email)},
                            u.street {nameof(UserTaskDto.Street)},
                            u.street_number {nameof(UserTaskDto.StreetNumber)},
                            u.street_complement {nameof(UserTaskDto.StreetComplement)},
                            u.neighborhood {nameof(UserTaskDto.Neighborhood)},
                            u.city {nameof(UserTaskDto.City)},
                            u.state {nameof(UserTaskDto.State)},
                            u.country {nameof(UserTaskDto.Country)},
                            u.zip_code {nameof(UserTaskDto.ZipCode)},
                            u.user_role {nameof(UserTaskDto.Role)},
                            u.manager_id {nameof(UserTaskDto.ManagerId)},
                            t.id {nameof(UserTaskDto.IdTask)},
                            t.user_id {nameof(UserTaskDto.UserIdTask)},
                            t.message {nameof(UserTaskDto.Message)},
                            t.deadline {nameof(UserTaskDto.Deadline)},
                            t.completed {nameof(UserTaskDto.Completed)}
                            from pastel.tb_user as u
                            left join pastel.tb_task as t on t.user_id = u.id 
                            where manager_id = '{managerId}'
                        ";

            return await _dbSession.Connection.QueryAsync<UserTaskDto>(query);
        }

        public async Task<IEnumerable<ManagersDto>> GetManagers()
        {
            var query = $@"
                            select 
                            id {nameof(ManagersDto.Id)},
                            first_name {nameof(ManagersDto.FirstName)},
                            last_name {nameof(ManagersDto.LastName)}
                            from pastel.tb_user
                            where user_role = '{Enum.GetName(Role.MANAGER)}'
                        ";

            return await _dbSession.Connection.QueryAsync<ManagersDto>(query);

        }

        public async Task<bool> FindEmail(string? email)
        {
            var query = $@"
                            select email {nameof(UserDto.Email)}
                            from pastel.tb_user
                            where email = '{email}'
                        ";
            var result = await _dbSession.Connection.QueryAsync<UserDto>(query);
            
            if (result.Any())
                return true;

            return false;
        }

        public async Task<bool> FindManager(Guid id)
        {
            var query = $@"
                            select user_role {nameof(UserDto.Role)}
                            from pastel.tb_user
                            where id = '{id}'
                        ";
            var result = await _dbSession.Connection.QueryAsync<UserDto>(query);

            var user = result.FirstOrDefault();
            Enum.TryParse<Role>(user?.Role, out var role);
            if (role != Role.MANAGER)
                return false;

            return true; 
        }

        public async Task<IEnumerable<UserDto>> GetUserByEmail(string? email)
        {
            var query = $@"
                            select 
                                id {nameof(UserDto.Id)},
                                first_name {nameof(UserDto.FirstName)},
                                last_name {nameof(UserDto.LastName)},
                                birth_date {nameof(UserDto.BirthDate)},
                                email {nameof(UserDto.Email)},
                                pass {nameof(UserDto.Password)},
                                street {nameof(UserDto.Street)},
                                street_number {nameof(UserDto.StreetNumber)},
                                street_complement {nameof(UserDto.StreetComplement)},
                                neighborhood {nameof(UserDto.Neighborhood)},
                                city {nameof(UserDto.City)},
                                state {nameof(UserDto.State)},
                                country {nameof(UserDto.Contry)},
                                zip_code {nameof(UserDto.ZipCode)},
                                user_role {nameof(UserDto.Role)},
                                manager_id {nameof(UserDto.ManagerId)}
                            from pastel.tb_user
                            where email = '{email}'
                        ";

            return await _dbSession.Connection.QueryAsync<UserDto>(query);
        }

        public async Task<IEnumerable<UserDto>> GetUserById(Guid? id)
        {
            var query = $@"
                            select 
                                id {nameof(UserDto.Id)},
                                first_name {nameof(UserDto.FirstName)},
                                last_name {nameof(UserDto.LastName)},
                                birth_date {nameof(UserDto.BirthDate)},
                                email {nameof(UserDto.Email)},
                                pass {nameof(UserDto.Password)},
                                street {nameof(UserDto.Street)},
                                street_number {nameof(UserDto.StreetNumber)},
                                street_complement {nameof(UserDto.StreetComplement)},
                                neighborhood {nameof(UserDto.Neighborhood)},
                                city {nameof(UserDto.City)},
                                state {nameof(UserDto.State)},
                                country {nameof(UserDto.Contry)},
                                zip_code {nameof(UserDto.ZipCode)},
                                user_role {nameof(UserDto.Role)},
                                manager_id {nameof(UserDto.ManagerId)}
                            from pastel.tb_user
                            where id = '{id}'
                        ";

            return await _dbSession.Connection.QueryAsync<UserDto>(query);

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
                                '{user.Address.Country}',
                                '{user.Address.ZipCode}',
                                '{Enum.GetName<Role>(user.Role)}',
                                '{user.ManagerId}'
                            );    
                        ";

                var result = await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);
                return result > 0;
        }

        public async Task Edit(User user)
        {
            var query = $@"
                            update pastel.tb_user
                             set
                             first_name = '{user.FullName.FirstName}',
                             last_name = '{user.FullName.LastName}',
                             birth_date = '{user.BirthDate.ToString("yyyyMMdd")}',
                             email = '{user.Email.Address}',
                             street = '{user.Address.Street}',
                             street_number = {user.Address.Number},
                             street_complement = '{user.Address.Complement}',
                             neighborhood = '{user.Address.Neighborhood}',
                             city = '{user.Address.City}',
                             state = '{user.Address.State}',
                             country = '{user.Address.Country}',
                             zip_code = '{user.Address.ZipCode}',
                             user_role = '{Enum.GetName<Role>(user.Role)}',
                             manager_id = '{user.ManagerId}'
                             where id = '{user.Id}'
                        ";

            await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);

        }

        public async Task Delete(Guid id)
        {
            var query = $@"
                            delete from pastel.tb_user
                            where id = '{id}'
                        ";

            await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);

        }

      
    }
}
