using Pastel.Domain.Validations;

namespace Pastel.Domain.Command
{
    public class DeleteCommand
    {
        public string? Id { get; set; }

        public bool IsValid() => new DeleteTaskCommandValidation().Validate(this).IsValid;

        public IEnumerable<string> Errors()
        {
            var validationResult = new DeleteTaskCommandValidation().Validate(this);

            foreach (var erro in validationResult.Errors)
            {
                yield return erro.ErrorMessage;
            }
        }
    }
}
