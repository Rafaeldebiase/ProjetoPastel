using Pastel.Domain.Entities;

namespace Pastel.Domain.Dto
{
    public record ResultDto
    {
        public ResultDto() 
        {
            Errors = new List<string?>();
        }

        public object? Obj { get; set; }

        public IList<string?> Errors { get; private set; }

        public void AddError(string error) => Errors.Add(error);

        public void AddObject(object obj)
        {
            Obj = obj;
        }


    }
}
