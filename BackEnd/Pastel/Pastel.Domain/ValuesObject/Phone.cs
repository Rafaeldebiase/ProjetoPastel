namespace Pastel.Domain.ValuesObject
{
    public record Phone
    {
        public Phone(string? type, string? number)
        {
            Type = type;
            Number = number;
        }

        public string? Type { get; set; }
        public string? Number { get; set; }
    }
}
