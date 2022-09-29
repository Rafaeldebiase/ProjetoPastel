using Pastel.Domain.Validations;

namespace Pastel.Domain.Command
{
    public class CreateTaskCommand
    {
        public string? UserId { get; set; }
        public string? Message { get; set; }
        public DateTime? Deadline { get; set; }

        public bool IsValid() => new CreateTaskCommandValidation().Validate(this).IsValid;

        public IEnumerable<string> Errors()
        {
            var validationResult = new CreateTaskCommandValidation().Validate(this);

            foreach (var erro in validationResult.Errors)
            {
                yield return erro.ErrorMessage;
            }
        }
    }
}
