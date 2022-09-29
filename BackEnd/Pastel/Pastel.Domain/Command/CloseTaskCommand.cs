using Pastel.Domain.Validations;

namespace Pastel.Domain.Command
{
    public record CloseTaskCommand
    {
        public string? Id { get; init; }

        public bool IsValid() => new CloseTaskCommandValidation().Validate(this).IsValid;

        public IEnumerable<string> Errors()
        {
            var validationResult = new CloseTaskCommandValidation().Validate(this);

            foreach (var erro in validationResult.Errors)
            {
                yield return erro.ErrorMessage;
            }
        }
    }
}
