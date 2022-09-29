namespace Pastel.Domain.Dto
{
    public record TaskDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string? Message { get; init; }
        public DateTime? Deadline { get; init; }
        public bool Completed { get; init; }
    }
}
