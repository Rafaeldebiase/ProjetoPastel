namespace Pastel.Domain.Entities
{
    public abstract record Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid? Id { get; private set; }

        public void ChangeId(Guid? id) => Id = id;
    }
}
