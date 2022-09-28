using Newtonsoft.Json.Linq;
using Pastel.Domain.Validations;
using Pastel.Domain.ValuesObject;

namespace Pastel.Domain.Command
{
    public record CreateUserCommand
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public List<Phone>? Phones { get; set; }
        public string? Street { get; set; }
        public int? StreetNumber { get; set; }
        public string? StreetComplement { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Contry { get; set; }
        public string? ZipCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Role { get; set; }
        public string? ManagerId { get; set; }

        public bool IsValid() => new CreateUserCommandValidation().Validate(this).IsValid;

        public IEnumerable<string> Errors()
        {
            var validationResult = new CreateUserCommandValidation().Validate(this);

            foreach (var erro in validationResult.Errors)
            {
                yield return erro.ErrorMessage;
            }
        }
       
    }
}
