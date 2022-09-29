using Pastel.Domain.Validations;

namespace Pastel.Domain.Command
{
    public record RemovePhoneCommand
    {
        public string? UserId { get; init; }
        public string? Number { get; init; }
        public string? Type { get; init; }

        public bool IsValid() => new RemovePhoneCommandValidation().Validate(this).IsValid;

        public IEnumerable<string> Errors()
        {
            var validationResult = new RemovePhoneCommandValidation().Validate(this);

            foreach (var erro in validationResult.Errors)
            {
                yield return erro.ErrorMessage;
            }
        }
    }
}
