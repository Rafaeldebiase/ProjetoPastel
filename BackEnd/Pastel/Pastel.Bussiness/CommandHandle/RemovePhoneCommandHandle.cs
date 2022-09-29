using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using Pastel.Domain.ValuesObject;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.CommandHandle
{
    public class RemovePhoneCommandHandle : IRemovePhoneCommandHandle
    {
        private readonly ILogger<RemovePhoneCommandHandle> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhoneRepository _repository;

        public RemovePhoneCommandHandle(ILogger<RemovePhoneCommandHandle> logger, IUnitOfWork unitOfWork, 
            IPhoneRepository repository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ResultDto> Remove(RemovePhoneCommand command)
        {
            var result = new ResultDto();
            try
            {
                Guid.TryParse(command.UserId, out var userId);
                var phone = new Phone(command.Type, command.Number);
                var userPhone = UserPhone.PhoneFactory.Generate(phone, userId);
                _unitOfWork.BeginTransaction();
                await _repository.Remove(userPhone);
                _unitOfWork.Commit();

                result.AddObject(phone);
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
