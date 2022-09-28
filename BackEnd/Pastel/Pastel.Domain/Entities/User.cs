using Pastel.Domain.Enums;
using Pastel.Domain.ValuesObject;

namespace Pastel.Domain.Entities
{
    public record User : Entity
    {
        public User(                
                FullName fullName, 
                DateTime? birthDate, 
                Email email, 
                Password password, 
                Address address,
                Role role,
                Guid? managerId
            )
        {
            FullName = fullName;

            if (birthDate.HasValue)
                BirthDate = birthDate.Value;

            Email = email;
            Password = password;
            Address = address;
            Role = role;
            ManagerId = managerId;
        }

        public FullName FullName { get; private init; }
        public DateTime BirthDate { get; private init; }
        public Email Email { get; private init; }
        public Password Password { get; private init; }
        public Address Address { get; private init; }
        public Role Role { get; private init; }
        public Guid? ManagerId { get; private init; }

        public User ChangeFullName(FullName fullName) =>
            this with { FullName = fullName };

        public User ChangeBirthDate(DateTime birthDate) =>
            this with { BirthDate = birthDate };

        public User ChangeEmail(Email email) =>
            this with { Email = email };

        public User ChangeFirstName(Password password) =>
            this with { Password = password };

        public User ChangeAddress(Address address) =>
            this with { Address = address };
    }
}
