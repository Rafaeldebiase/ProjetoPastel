using Pastel.Domain.Entities;

namespace Pastel.Domain.Dto
{
    public class ResultUserDto
    {
        public ResultUserDto()
        {
            Errors = new List<string?>();
        }

        public UserDto? User { get; set; }

        public IList<string?> Errors { get; private set; }

        public void AddError(string error) => Errors.Add(error);

        public void AddUser(UserDto user)
        {
            User = user;
        }
    }
}
