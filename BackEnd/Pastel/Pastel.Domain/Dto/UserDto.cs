using Pastel.Domain.Entities;
using Pastel.Domain.Enums;

namespace Pastel.Domain.Dto
{
    public record UserDto
    {
        public UserDto() {}

        protected UserDto(string? firstName, string? lastName, string? role, Guid? id)
        {
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Id = id;
        }

        protected UserDto(
                Guid? id, 
                string? firstName, 
                string? lastName, 
                string? email, 
                string? street, 
                int? streetNumber, 
                string? streetComplement, 
                string? neighborhood, 
                string? city, string? 
                state, string? contry, 
                string? zipCode, 
                DateTime? birthDate, 
                string? role, 
                Guid? managerId
            )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Street = street;
            StreetNumber = streetNumber;
            StreetComplement = streetComplement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Contry = contry;
            ZipCode = zipCode;
            BirthDate = birthDate;
            Role = role;
            ManagerId = managerId;
        }

        public Guid? Id { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? Password { get; init; }
        public string? Street { get; init; }
        public int? StreetNumber { get; init; }
        public string? StreetComplement { get; init; }
        public string? Neighborhood { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public string? Contry { get; init; }
        public string? ZipCode { get; init; }
        public DateTime? BirthDate { get; init; }
        public string? Role { get; init; }
        public Guid? ManagerId { get; init; }

        public static class UserDtoFactory
        {
            public static UserDto GenerateFromUser(User user)
            {
                return new UserDto(
                       user.Id,
                       user.FullName.FirstName,
                       user.FullName.LastName,
                       user.Email.Address,
                       user.Address.Street,
                       user.Address.Number,
                       user.Address.Complement,
                       user.Address.Neighborhood,
                       user.Address.City,
                       user.Address.State,
                       user.Address.Country,
                       user.Address.ZipCode,
                       user.BirthDate,
                       Enum.GetName<Role>(user.Role),
                       user.ManagerId
                   ); 
            }

            public static UserDto GenerateFromUserTaskDto(UserTaskDto user)
            {
                return new UserDto(
                       user.Id,
                       user.FirstName,
                       user.LastName,
                       user.Email,
                       user.Street,
                       user.StreetNumber,
                       user.StreetComplement,
                       user.Neighborhood,
                       user.City,
                       user.State,
                       user.Country,
                       user.ZipCode,
                       user.BirthDate,
                       user.Role,
                       user.ManagerId
                   );
            }

            public static UserDto GenerateToAuth(string? firstName, string? lastName, string? role, Guid? id) =>
                new(firstName, lastName, role, id);
               
        }
    }
}
