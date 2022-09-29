using FluentValidation;
using Pastel.Domain.Command;

namespace Pastel.Domain.Validations
{
    public class AutenticateCommandValidation : AbstractValidator<AutenticateCommand>
    {
        public AutenticateCommandValidation()
        {
            RuleFor(field => field.Email)
               .NotEmpty().WithMessage("O campo email não pode ser vazio")
               .EmailAddress();

            RuleFor(field => field.Password)
                .NotEmpty().WithMessage("O campo senha não pode ser vazio");
        }
    }
}
