namespace WebRozetka.Data.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime DateCreated { get; set; }
    }

    public class BaseEntity<T> : IEntity<T>
    {
        public T Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateCreated { get; set; }
    }
}
