namespace Pastel.Domain.ValuesObject
{
    public record Address
    {
        public Address(
                string? street,
                int? number,
                string? complement,
                string? neighborhood,
                string? city,
                string? state,
                string? contry,
                string? zipCode
            )
        {
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Contry = contry;
            ZipCode = zipCode;
        }
        
        public string? Street { get; private init; }
        public int? Number { get; private init; }
        public string? Complement { get; private init; }
        public string? Neighborhood { get; private init; }
        public string? City { get; private init; }
        public string? State { get; private init; }
        public string? Contry { get; private init; }
        public string? ZipCode { get; private init; }

        public Address ChangeStreet(string street) =>
            this with { Street = street };

        public Address ChangeNumber(int? number) =>
            this with { Number = number };

        public Address ChangeComplement(string complement) =>
            this with { Complement = complement };

        public Address ChangeNeighborhood(string neighborhood) =>
            this with { Neighborhood = neighborhood };

        public Address ChangeCity(string city) =>
            this with { City = city };

        public Address ChangeState(string state) =>
            this with { State = state };

        public Address ChangeCountry(string country) =>
            this with { Contry = country };

        public Address ChangeZipCode(string zipCode) =>
            this with { ZipCode = zipCode };


    }
}
