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
    public class AddPhoneCommandHandle : IAddPhoneCommandHandle
    {
        private readonly ILogger<AddPhoneCommandHandle> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhoneRepository _repository;

        public AddPhoneCommandHandle(ILogger<AddPhoneCommandHandle> logger, IUnitOfWork unitOfWork, 
            IPhoneRepository repository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ResultPhoneDto> Add(AddPhoneCommand command)
        {
            var phoneList = new ResultPhoneDto();
            try
            {
                foreach (var item in command.Phones)
                {
                    var phone = new Phone(item.Type, item.Number);
                    Guid.TryParse(command.UserId, out var userId);
                    var userPhone = UserPhone.PhoneFactory.Generate(phone, userId);

                    _unitOfWork.BeginTransaction();
                    await _repository.Ingestion(userPhone);
                    _unitOfWork.Commit();
                    phoneList.AddPhone(item);
                }
                return phoneList;
            }
            catch (Exception error)
            {
                _unitOfWork.Rollback();
                var message = $"{error.InnerException}\n " +
                    $"{error.Message} \n " +
                    $"{error.StackTrace}";

                _logger.LogError(message);

                phoneList.AddErrors(message);

                return phoneList;
            }
        }
    }
}
