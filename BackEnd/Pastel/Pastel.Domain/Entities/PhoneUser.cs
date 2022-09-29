using Pastel.Domain.Command;
using Pastel.Domain.Enums;
using Pastel.Domain.ValuesObject;

namespace Pastel.Domain.Entities
{
    public record UserPhone : Entity
    {
        protected UserPhone(PhoneType? type, string? number, Guid? userId)
        {
            Type = type;
            Number = number;
            UserId = userId;
        }

        public Guid? UserId { get; private init; }
        public PhoneType? Type { get; private init; }
        public string? Number { get; private init; }


        public static class PhoneFactory
        {
            public static IEnumerable<UserPhone> Create(IEnumerable<Phone>? phones, Guid? userId)
            {
                foreach (var phone in phones)
                {
                    Enum.TryParse<PhoneType>(phone.Type, out var type);

                    yield return new(type, phone.Number, userId);
                }
            }
                

            public static IEnumerable<UserPhone> ChangePhone(List<UserPhone> phones, UserPhone oldPhone, 
                string newNumber, PhoneType newType, Guid userId)
            {
                phones.Remove(oldPhone);
                phones.Add(new(newType, newNumber, userId));
                return phones;
            }
        }
    }
}