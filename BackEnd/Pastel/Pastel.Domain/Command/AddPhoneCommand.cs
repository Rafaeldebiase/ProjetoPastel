using Pastel.Domain.Dto;
using Pastel.Domain.Validations;

namespace Pastel.Domain.Command
{
    public record AddPhoneCommand
    {
        public string? UserId { get; init; }
        public List<PhoneDto>? Phones { get; init; }

        public bool IsValid() => new AddPhoneCommandValidation().Validate(this).IsValid;

        public IEnumerable<string> Errors()
        {
            var validationResult = new AddPhoneCommandValidation().Validate(this);

            foreach (var erro in validationResult.Errors)
            {
                yield return erro.ErrorMessage;
            }
        }
    }
}
