using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.CommandHandle
{
    public class EditTaskCommandHandle : IEditTaskCommandHandle
    {
        private readonly ILogger<EditTaskCommandHandle> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _repository;

        public EditTaskCommandHandle(ILogger<EditTaskCommandHandle> logger, IUnitOfWork unitOfWork, 
            ITaskRepository repository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ResultDto> Edit(EditTaskCommand command)
        {
            var result = new ResultDto();
            try
            {
                Guid.TryParse(command.Id, out var id);
                var taskDto = await _repository.GetTaskById(id);
                var task = Check(taskDto.FirstOrDefault(), command);

                _unitOfWork.BeginTransaction();

                _unitOfWork.Commit();
                await _repository.Edit(task);
                result.AddObject(task);

                return result;
            }
            catch (Exception error)
            {
                _unitOfWork.Rollback();
                var message = $"{error.InnerException}\n " +
                    $"{error.Message} \n " +
                    $"{error.StackTrace}";

                _logger.LogError(message);

                result.AddError(message);

                return result;
            }



        }

        private TaskModel Check(TaskDto? dto, EditTaskCommand command)
        {
            var task = TaskModel.TaskModelFactory.Generate(dto);

            Guid.TryParse(command.UserId, out var userId);
            if (!userId.Equals(task.UserId))
            {
                task = task.ChangeUserId(userId);
            }

            if (command.Message != task.Message)
            {
                task = task.ChangeMessage(command.Message);
            }

            if (command.Deadline.HasValue)
            {
                if (!command.Deadline.Value.Equals(task.Deadline))
                {
                    task = task.ChangeDeadline(command.Deadline);
                }
            }

            if (command.Completed != task.Completed)
            {
                task = task.ChangeCompleted(command.Completed);
            }

            return task;
        }
    }
}
