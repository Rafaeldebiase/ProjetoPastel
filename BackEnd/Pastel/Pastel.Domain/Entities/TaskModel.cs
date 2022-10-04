using Pastel.Domain.Dto;

namespace Pastel.Domain.Entities
{
    public record TaskModel: Entity
    {
        protected TaskModel(string? message, DateTime? deadline, Guid? userId, bool? completed)
        {
            Message = message;
            Deadline = deadline;
            UserId = userId;
            Completed = completed;
        }

        public Guid? UserId { get; private init; }
        public string? Message { get; private init; }
        public DateTime? Deadline { get; private init; }
        public bool? Completed { get; private init; }

        public TaskModel ChangeMessage(string? message) =>
            this with { Message = message };

        public TaskModel ChangeDeadline(DateTime? deadline) =>
            this with { Deadline = deadline };

        public TaskModel ChangeUserId(Guid id) =>
            this with { UserId = id };

        public TaskModel ChangeCompleted(bool? completed) =>
             this with { Completed = completed };

        public static class TaskModelFactory
        {
            public static TaskModel Generate(string? mensagem, DateTime? deadline, Guid id, bool completed = false) =>  
                new TaskModel(mensagem, deadline, id, completed);

            public static TaskModel Generate(string? mensagem, DateTime? deadline, Guid? id, Guid? userId ,bool completed = false)
            {
                var task = new TaskModel(mensagem, deadline, userId, completed);
                task.ChangeId(id);
                return task;
            }

                

            public static TaskModel Generate(TaskDto? dto)
            {
                var task = new TaskModel(dto?.Message, dto?.Deadline, dto?.UserId, dto?.Completed);
                task.ChangeId(dto?.Id);
                return task;
            }
        }
    }
}
