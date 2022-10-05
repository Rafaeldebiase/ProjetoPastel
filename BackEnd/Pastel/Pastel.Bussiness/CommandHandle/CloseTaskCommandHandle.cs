using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.CommandHandle
{
    public class CloseTaskCommandHandle : ICloseTaskCommandHandle
    {
        private readonly ILogger<CloseTaskCommandHandle> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _repository;
        private readonly IEmailHandle _emailHandle;
        private readonly IUserRepository _userRepository;

        public CloseTaskCommandHandle(ILogger<CloseTaskCommandHandle> logger, IUnitOfWork unitOfWork,
            ITaskRepository repository, IEmailHandle emailHandle, IUserRepository userRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _emailHandle = emailHandle;
            _userRepository = userRepository;
        }

        public async Task<ResultDto> Close(CloseTaskCommand command)
        {
            var result = new ResultDto();
            try
            {
                Guid.TryParse(command.Id, out var id);
                var tasksDto = await _repository.GetTaskById(id);

                if (tasksDto.Count() <= 0)
                {
                    result.AddError("Tarefa não encontrada");
                    return result;
                }

                var task = TaskModel.TaskModelFactory.Generate(tasksDto.FirstOrDefault());
                var usersDto = await _userRepository.GetUserById(task.UserId);
                var user = User.UserFactory.Generate(usersDto.FirstOrDefault());
                var managersDto = await _userRepository.GetUserById(user.ManagerId);

                var taskStatusChanged = task.ChangeCompleted(command.Completed);

                _unitOfWork.BeginTransaction();
                await _repository.Close(taskStatusChanged);
                _unitOfWork.Commit();

                if(managersDto.Any())
                {
                    var manager = User.UserFactory.Generate(managersDto.FirstOrDefault());
                    var template = _emailHandle.CompletedTaskEmailTemplate(task, manager.FullName, user.FullName);
                    var subject = "Alteração de status";
                    _emailHandle.Send(manager.Email.Address, subject, template);
                }

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
    }
}
