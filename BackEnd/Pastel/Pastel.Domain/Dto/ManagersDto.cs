namespace Pastel.Domain.Dto
{
    public record ManagersDto
    {
        public Guid Id { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }

    }
}
