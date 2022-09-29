using Pastel.Domain.Validations;

namespace Pastel.Domain.Command
{
    public class AutenticateCommand
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public bool IsValid() => new AutenticateCommandValidation().Validate(this).IsValid;

        public IEnumerable<string> Errors()
        {
            var validationResult = new AutenticateCommandValidation().Validate(this);

            foreach (var erro in validationResult.Errors)
            {
                yield return erro.ErrorMessage;
            }
        }
    }
}
