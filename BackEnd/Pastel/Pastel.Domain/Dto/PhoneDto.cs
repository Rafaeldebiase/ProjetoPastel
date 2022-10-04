namespace Pastel.Domain.Dto
{
    public record PhoneDto
    {
        public PhoneDto(string? number, string? type)
        {
            Number = number;
            Type = type;
        }

        public string? Number { get; init; }
        public string? Type { get; init; }
    }
}
