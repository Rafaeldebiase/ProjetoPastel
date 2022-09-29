using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.CommandHandle
{
    public class DeleteUserCommandHandle : IDeleteUserCommandHandle
    {
        private readonly ILogger<DeleteUserCommandHandle> _logger;
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandle(ILogger<DeleteUserCommandHandle> logger, IUserRepository repository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto> Delete(DeleteUserCommand command)
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
