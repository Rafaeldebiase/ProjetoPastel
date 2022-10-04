using Pastel.Domain.Entities;

namespace Pastel.Domain.Dto
{
    public record ResultUserTaskDto
    {
        public ResultUserTaskDto(UserDto? userDto)
        {
            UserDto = userDto;
            Task = new List<TaskModel>();
        }

        public UserDto? UserDto { get; init; }
        public List<TaskModel>? Task { get; private set; }

        public void AddTask(IEnumerable<TaskModel> task)
        {
            Task?.AddRange(task);
        }
    }
}
