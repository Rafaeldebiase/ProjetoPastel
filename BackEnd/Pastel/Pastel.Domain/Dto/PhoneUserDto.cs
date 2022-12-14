using Pastel.Domain.Enums;

namespace Pastel.Domain.Dto
{
    public record PhoneUserDto
    {
        public Guid Id { get; init; }
        public Guid? UserId { get; init; }
        public PhoneType Type { get; init; }
        public string? Number { get; init; }
    }
}
