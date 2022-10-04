using FluentValidation;
using Pastel.Domain.Command;
using Pastel.Domain.Enums;

namespace Pastel.Domain.Validations
{
    public class AddPhoneCommandValidation  : AbstractValidator<AddPhoneCommand>
    {
        public AddPhoneCommandValidation()
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
        }
    }
}
