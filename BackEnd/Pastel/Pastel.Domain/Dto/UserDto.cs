namespace Pastel.Domain.Dto
{
    public record UserDto
    {
        public Guid Id { get; init; }
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
        public Guid ManagerId { get; init; }
    }
}
