using FluentValidation;
using Pastel.Domain.Command;
using Pastel.Domain.Enums;
using Pastel.Domain.ValuesObject;

namespace Pastel.Domain.Validations
{
    public class UserCreateCommandValidation : AbstractValidator<CreateUserCommand>
    {
        public UserCreateCommandValidation()
        {
            RuleFor(field => field.FirstName)
                .NotEmpty().WithMessage("O campo primeiro nome não pode ser vazio");

            RuleFor(field => field.LastName)
                .NotEmpty().WithMessage("O campo segundo nome não pode ser vazio");

            RuleFor(field => field.BirthDate)
                .NotEmpty().WithMessage("A data de nascimento não estar vazia");

            RuleFor(field => field.Email)
                .NotEmpty().WithMessage("O campo email não pode ser vazio")
                .EmailAddress();

            RuleFor(field => field.Password)
                .NotEmpty().WithMessage("O campo senha não pode ser vazio");

            RuleFor(field => field.Street)
                .NotEmpty().WithMessage("O campo logradouro não pode ser vazio");

            RuleFor(field => field.StreetNumber)
                .NotEmpty().WithMessage("O campo numero da rua não pode ser vazio");

            RuleFor(field => field.Neighborhood)
                .NotEmpty().WithMessage("O campo bairro não pode ser vazio");

            RuleFor(field => field.City)
                .NotEmpty().WithMessage("O campo cidade não pode ser vazio");

            RuleFor(field => field.State)
                .NotEmpty().WithMessage("O campo estado não pode ser vazio");

            RuleFor(field => field.Contry)
                .NotEmpty().WithMessage("O campo pais não pode ser vazio");

            RuleFor(field => field.Role)
                .NotEmpty().WithMessage("O campo função não pode ser vazio")
                .IsEnumName(typeof(Role), false);

            RuleFor(field => field)
                .Custom((field, context) =>
                {
                    if(field.Phones is not null)
                    {
                        if (field.Phones?.Count > 0)
                        {
                            foreach(var phone in field.Phones)
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
                    if(field.Role == Enum.GetName<Role>(Role.USER))
                    {
                        if(string.IsNullOrEmpty(field.ManagerId))
                            context.AddFailure("O campo id do gestor não foi informado");

                        var result = Guid.TryParse(field.ManagerId, out var managerId);
                        if (!result)
                            context.AddFailure("O id do gestor não é um Guid");
                        
                    }
                });
        }
    }
}
