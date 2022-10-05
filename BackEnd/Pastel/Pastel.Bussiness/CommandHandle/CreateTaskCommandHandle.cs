using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.CommandHandle
{
    public class CreateTaskCommandHandle : ICreateTaskCommandHandle
    {
        private readonly ILogger<CreateTaskCommandHandle> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailHandle _emailHandle;

        public CreateTaskCommandHandle(ILogger<CreateTaskCommandHandle> logger, IUnitOfWork unitOfWork,
            ITaskRepository repository, IUserRepository userRepository, IEmailHandle emailHandle)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _userRepository = userRepository;
            _emailHandle = emailHandle;
        }

        public async Task<ResultDto> Create(CreateTaskCommand command)
        {
            var result = new ResultDto();
            try
            {
                Guid.TryParse(command.UserId, out var id);
                var usersDto = await _userRepository.GetUserById(id);

                if (usersDto.Count() <= 0)
                {
                    result.AddError("Usuário não encontrado");
                    return result;
                }

                var user = User.UserFactory.Generate(usersDto.FirstOrDefault());

                if (command.Deadline.HasValue)
                {
                    var task = TaskModel.TaskModelFactory.Generate(command.Message, command.Deadline.Value, id);
                    _unitOfWork.BeginTransaction();
                    await _repository.Create(task);
                    _unitOfWork.Commit();

                    var template = _emailHandle.CreatedTaskEmailTemplate(task, user.FullName);
                    var subject = "Nova tarefa";
                    _emailHandle.Send(user.Email.Address, subject, template);

                    result.AddObject(task);
                    return result;
                }

                result.AddError("O deadline não foi informado");
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
