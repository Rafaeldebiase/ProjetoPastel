namespace Pastel.Domain.Entities
{
    public record Photo : Entity
    {
        public Photo(
                string name, 
                byte[] data, 
                string contentType, 
                Guid userId
            )
        {
            Name = name;
            Data = data;
            ContentType = contentType;
            UserId = userId;
        }

        public string Name { get; private init; }
        public byte[] Data { get; private init; }
        public string ContentType { get; private init; }
        public Guid UserId { get; private init; }
    }
}
