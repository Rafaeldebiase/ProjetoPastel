using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Domain.Dto;
using Pastel.Domain.Enums;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.Handle
{
    public class PhoneHandle : IPhoneHandle
    {
        private readonly ILogger<PhoneHandle> _logger;
        private readonly IPhoneRepository _repository;

        public PhoneHandle(ILogger<PhoneHandle> logger, IPhoneRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<ResultPhoneDto> GetPhonesByUserId(Guid userId)
        {
            var result = new ResultPhoneDto();
            try
            {
                var phonesDto = await _repository.GetPhonesByUserId(userId);

                var phones = phonesDto.Select(x =>
                {
                    var type = Enum.GetName<PhoneType>(x.Type);
                    return new PhoneDto(x.Number, type);
                });

                result.AddPhones(phones);

                return result;

            }
            catch (Exception error)
            {
                var message = $"{error.InnerException}\n " +
                    $"{error.Message} \n " +
                    $"{error.StackTrace}";

                _logger.LogError(message);

                result.AddErrors(message);

                return result;
            }




        }
    }
}
