using FluentValidation;
using Pastel.Domain.Command;

namespace Pastel.Domain.Validations
{
    public class DeleteTaskCommandValidation : AbstractValidator<DeleteCommand>
    {
        public DeleteTaskCommandValidation()
        {
            RuleFor(field => field.Id)
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
