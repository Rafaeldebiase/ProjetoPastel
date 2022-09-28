namespace Pastel.Domain.Dto
{
    public record PhotoDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public byte[]? Data { get; init; }
        public string? ContentType { get; init; }
        public Guid UserId { get; init; }
    }
}
