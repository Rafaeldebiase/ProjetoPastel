using Dapper;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;

namespace Pastel.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DbSession _dbSession;

        public TaskRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<IEnumerable<TaskDto>> GetTaskByUserId(Guid userId)
        {
            var query = $@"
                            select 
                            id {nameof(TaskDto.Id)},
                            user_id {nameof(TaskDto.UserId)},
                            message {nameof(TaskDto.Message)},
                            deadline {nameof(TaskDto.Deadline)},
                            completed {nameof(TaskDto.Completed)}
                            from pastel.tb_task
                            where user_id = '{userId}'
                        ";

            return await _dbSession.Connection.QueryAsync<TaskDto>(query);

        }

        public async Task<IEnumerable<TaskDto>> GetTaskById(Guid id)
        {
            var query = $@"
                            select 
                            id {nameof(TaskDto.Id)},
                            user_id {nameof(TaskDto.UserId)},
                            message {nameof(TaskDto.Message)},
                            deadline {nameof(TaskDto.Deadline)}
                            from pastel.tb_task
                            where id = '{id}'
                        ";

            return await _dbSession.Connection.QueryAsync<TaskDto>(query);

        }

        public async Task<bool> Close(TaskModel task)
        {
            var query = $@"
                            update pastel.tb_task
                            set completed = {task.Completed}
                            where id = '{task.Id}'
                        ";

            var result = await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);
            return result > 0;
        }

        public async Task<bool> Create(TaskModel task)
        {
            var query = $@"
                            insert into pastel.tb_task
                            (
                                id,
                                user_id,
                                message,
                                deadline,
                                completed
                            )values(
                                '{task.Id}',
                                '{task.UserId}',
                                '{task.Message}',
                                '{task.Deadline?.ToString("yyyyMMdd")}',
                                {task.Completed}
                            )
                        ";

            var result = await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);
            return result > 0;
        }

        public async Task<bool> Edit(TaskModel task)
        {
            var query = $@"
                            update pastel.tb_task
                            set 
                            user_id = '{task.UserId}',
                            message = '{task.Message}',
                            deadline = '{task.Deadline?.ToString("yyyyMMdd")}',
                            completed = {task.Completed}
                            where id = '{task.Id}'
                        ";

            var result = await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);
            return result > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var query = $@"
                            delete from pastel.tb_task
                            where id = '{id}'
                        ";

            var result = await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);
            return result > 0;
        }
    }
}
