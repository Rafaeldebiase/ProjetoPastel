namespace Pastel.Domain.ValuesObject
{
    public record Email
    {
        public Email(string? address)
        {
            Address = address;
        }

        public string? Address { get; private init; }

        public Email ChangeAddress(string address) =>
            this with { Address = address };
    }
}
