namespace Pastel.Domain.ValuesObject
{
    public record Password
    {
        public Password(string? code)
        {
            Code = code;
        }

        public string? Code { get; private init; }

        public Password ChangeCode(string code) =>
            this with { Code = code };
    }
}
