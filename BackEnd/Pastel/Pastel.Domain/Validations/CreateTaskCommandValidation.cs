using FluentValidation;
using Pastel.Domain.Command;

namespace Pastel.Domain.Validations
{
    public class CreateTaskCommandValidation : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidation()
        {
            RuleFor(field => field.UserId)
                 .Custom((id, context) =>
                 {
                     if (string.IsNullOrEmpty(id))
                         context.AddFailure("O campo id do usuário não foi informado");

                     var result = Guid.TryParse(id, out var userId);
                     if (!result)
                         context.AddFailure("O id do usuário não é um Guid");
                 });

            RuleFor(field => field.Message)
               .NotEmpty().WithMessage("O campo mensagem não pode ser vazio");

            RuleFor(field => field.Deadline)
                .NotEmpty().WithMessage("O campo deadline não pode ser vazio");
        }
    }
}
