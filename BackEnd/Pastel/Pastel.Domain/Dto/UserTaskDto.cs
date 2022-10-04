namespace Pastel.Domain.Dto
{
    public class UserTaskDto
    {
        public Guid? Id { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? Street { get; init; }
        public int? StreetNumber { get; init; }
        public string? StreetComplement { get; init; }
        public string? Neighborhood { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public string? Country { get; init; }
        public string? ZipCode { get; init; }
        public DateTime? BirthDate { get; init; }
        public string? Role { get; init; }
        public Guid? ManagerId { get; init; }
        public Guid? IdTask { get; init; }
        public Guid? UserIdTask { get; init; }
        public string? Message { get; init; }
        public DateTime? Deadline { get; init; }
        public string? Completed { get; init; }
    }
}
