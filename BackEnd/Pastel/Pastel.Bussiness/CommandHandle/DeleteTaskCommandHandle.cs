using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.CommandHandle
{
    public class DeleteTaskCommandHandle : IDeleteTaskCommandHandle
    {
        private readonly ILogger<DeleteTaskCommandHandle> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _repository;

        public DeleteTaskCommandHandle(ILogger<DeleteTaskCommandHandle> logger, IUnitOfWork unitOfWork,
            ITaskRepository repository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ResultDto> Delete(DeleteCommand command)
        {
            var result = new ResultDto();
            try
            {
                Guid.TryParse(command.Id, out var userId);
                _unitOfWork.BeginTransaction();
                await _repository.Delete(userId);
                _unitOfWork.Commit();

                result.AddObject(command);

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
