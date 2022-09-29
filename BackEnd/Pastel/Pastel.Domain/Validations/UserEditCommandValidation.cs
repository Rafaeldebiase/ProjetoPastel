using FluentValidation;
using Pastel.Domain.Command;
using Pastel.Domain.Enums;

namespace Pastel.Domain.Validations
{
    public class UserEditCommandValidation : AbstractValidator<UserEditCommand>
    {
        public UserEditCommandValidation()
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

            RuleFor(field => field)
                .Custom((field, context) =>
                {
                    if (field.Phones is not null)
                    {
                        if (field.Phones?.Count > 0)
                        {
                            foreach (var phone in field.Phones)
                            {
                                if (!string.IsNullOrEmpty(phone?.Number))
                                {
                                    var check = Enum.TryParse<PhoneType>(phone.Type, false, out var typePhone);
                                    if (!check)
                                        context.AddFailure("O campo tipo do telefone não foi informado corretamente");
                                }
                            }
                        }
                    }
                });

            RuleFor(field => field)
                .Custom((field, context) =>
                {
                    if (!string.IsNullOrEmpty(field.Role))
                    {
                        if (field.Role == Enum.GetName<Role>(Role.USER))
                        {
                            if (string.IsNullOrEmpty(field.ManagerId))
                                context.AddFailure("O campo id do gestor não foi informado");

                            var result = Guid.TryParse(field.ManagerId, out var managerId);
                            if (!result)
                                context.AddFailure("O id do gestor não é um Guid");

                        }
                    }
                });
        }
    }
}
