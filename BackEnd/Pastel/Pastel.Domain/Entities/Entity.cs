namespace Pastel.Domain.Entities
{
    public abstract record Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private init; }
    }
}
