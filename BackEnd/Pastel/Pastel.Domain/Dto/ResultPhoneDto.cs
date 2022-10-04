namespace Pastel.Domain.Dto
{
    public record ResultPhoneDto
    {
        public ResultPhoneDto()
        {
            Phones = new List<PhoneDto>();
            Errors = new List<string>();
        }

        public List<PhoneDto> Phones { get; private set; }
        public List<string> Errors { get; private set; }

        public void AddPhone(PhoneDto dto)
        {
            Phones.Add(dto);
        }

        public void AddPhones(IEnumerable<PhoneDto> dto)
        {
            Phones.AddRange(dto);
        }

        public void AddErrors(string erro)
        {
            Errors.Add(erro);
        }
    }
}
