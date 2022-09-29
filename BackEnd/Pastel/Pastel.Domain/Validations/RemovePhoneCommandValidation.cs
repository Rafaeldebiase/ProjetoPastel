using FluentValidation;
using Pastel.Domain.Command;
using Pastel.Domain.Enums;

namespace Pastel.Domain.Validations
{
    public class RemovePhoneCommandValidation : AbstractValidator<RemovePhoneCommand>
    {
        public RemovePhoneCommandValidation()
        {
            RuleFor(field => field.UserId)
                .Custom((id, context) =>
                {
                    if (string.IsNullOrEmpty(id))
                        context.AddFailure("O campo id não foi informado");

                    var result = Guid.TryParse(id, out var userId);
                    if (!result)
                        context.AddFailure("O id não é um Guid");
                });

            RuleFor(field => field.Number)
                .NotEmpty().WithMessage("O campo logradouro não pode ser vazio");

            RuleFor(field => field.Type)
                .NotEmpty().WithMessage("O campo função não pode ser vazio")
                .IsEnumName(typeof(PhoneType), false);
        }
    }
}
