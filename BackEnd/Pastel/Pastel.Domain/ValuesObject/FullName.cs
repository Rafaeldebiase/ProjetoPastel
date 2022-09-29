namespace Pastel.Domain.ValuesObject
{
    public record FullName
    {
        public FullName(string? firstName, string? lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string? FirstName { get; private init; }
        public string? LastName { get; private init; }

        public FullName ChangeFirstName(string? firstName) =>
            this with { FirstName = firstName };

        public FullName ChangeLastName(string? lastName) =>
            this with { LastName = lastName };

    }
}
