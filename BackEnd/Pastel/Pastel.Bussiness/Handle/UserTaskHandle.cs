using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.Handle
{
    public class UserTaskHandle : IUserTaskHandle
    {
        private readonly ILogger<UserTaskHandle> _logger;
        private readonly IUserRepository _repository;

        public UserTaskHandle(ILogger<UserTaskHandle> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<IEnumerable<ResultUserTaskDto>> GetUsers(Guid managerId)
        {
            var usersTaskDto = await _repository.GetUsers(managerId);

            var results = new List<ResultUserTaskDto>();

            foreach (var item in usersTaskDto)
            {
                var userDto = UserDto.UserDtoFactory.GenerateFromUserTaskDto(item);
                var userTaskDto = new ResultUserTaskDto(userDto);
                var check = results.Where(x => x.UserDto?.Id == item.Id).Any();
                if(!check)
                    results.Add(userTaskDto);
            }

            foreach (var result in results)
            {
                var userTask = usersTaskDto.Where(x => x.UserIdTask == result.UserDto?.Id);
                if(userTask.Any())
                {
                    var tasks = userTask.Select(x =>
                    {
                        DateTime? deadline = DateTime.Now;
                        
                        if (x.Deadline.HasValue)
                        {
                            deadline = x.Deadline;
                        }
                        Boolean.TryParse(x.Completed, out var completed);
                        return TaskModel.TaskModelFactory.Generate(x.Message, deadline,
                                                                    x.IdTask, x.UserIdTask, completed);
                    });

                    result.AddTask(tasks);
                }
                
            }

            return results;
        }
    }
}
