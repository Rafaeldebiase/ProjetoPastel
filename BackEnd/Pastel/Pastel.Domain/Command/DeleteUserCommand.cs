using Pastel.Domain.Validations;

namespace Pastel.Domain.Command
{
    public class DeleteUserCommand
    {
        public string? Id { get; init; }

        public bool IsValid() => new DeleteUserCommandValidation().Validate(this).IsValid;

        public IEnumerable<string> Errors()
        {
            var validationResult = new DeleteUserCommandValidation().Validate(this);

            foreach (var erro in validationResult.Errors)
            {
                yield return erro.ErrorMessage;
            }
        }
    }
}
