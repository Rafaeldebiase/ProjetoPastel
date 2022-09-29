using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Domain.Enums;
using Pastel.Domain.ValuesObject;

namespace Pastel.Domain.Entities
{
    public record User : Entity
    {
        protected User(                
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

        public User ChangePassword(Password password) =>
            this with { Password = password };

        public User ChangeAddress(Address address) =>
            this with { Address = address };

        public User ChangeRole(Role role) =>
            this with { Role = role };

        public User ChangeManagerId(Guid id) =>
            this with { ManagerId = id };

        public static class UserFactory
        {
            public static User Generate(CreateUserCommand obj)
            {
                var fullName = new FullName(obj.FirstName, obj.LastName);
                var email = new Email(obj.Email);
                var password = new Password(obj.Password);
                var address = new Address(
                        obj.Street,
                        obj.StreetNumber,
                        obj.StreetComplement,
                        obj.Neighborhood,
                        obj.City,
                        obj.State,
                        obj.Contry,
                        obj.ZipCode
                    );
                Enum.TryParse<Role>(obj.Role, out var role);
                Guid.TryParse(obj.ManagerId, out var managerId);

                return new(
                        fullName,
                        obj.BirthDate,
                        email,
                        password,
                        address,
                        role,
                        managerId
                    );
            }

            public static User Generate(UserDto? obj)
            {
                var fullName = new FullName(obj?.FirstName, obj?.LastName);
                var email = new Email(obj?.Email);
                var password = new Password(obj?.Password);
                var address = new Address(
                        obj?.Street,
                        obj?.StreetNumber,
                        obj?.StreetComplement,
                        obj?.Neighborhood,
                        obj?.City,
                        obj?.State,
                        obj?.Contry,
                        obj?.ZipCode
                    );
                Enum.TryParse<Role>(obj?.Role, out var role);

                var user = new User(
                        fullName,
                        obj?.BirthDate,
                        email,
                        password,
                        address,
                        role,
                        obj?.ManagerId
                    );
                user.ChangeId(obj?.Id);

                return user;
            }
        }
    }
}
