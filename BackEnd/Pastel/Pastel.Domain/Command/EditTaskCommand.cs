using Pastel.Domain.Validations;

namespace Pastel.Domain.Command
{
    public class EditTaskCommand
    {
        public string? Id { get; init; }
        public string? UserId { get; init; }
        public string? Message { get; init; }
        public DateTime? Deadline { get; init; }
        public bool? Completed { get; set; }

        public bool IsValid() => new EditTaskCommandValidation().Validate(this).IsValid;

        public IEnumerable<string> Errors()
        {
            var validationResult = new EditTaskCommandValidation().Validate(this);

            foreach (var erro in validationResult.Errors)
            {
                yield return erro.ErrorMessage;
            }
        }
    }
}
