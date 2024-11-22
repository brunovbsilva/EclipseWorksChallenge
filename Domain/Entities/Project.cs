namespace Domain.Entities
{
    public class Project : BaseEntity
    {
        protected Project() {}
        public virtual IEnumerable<Task> Tasks { get; private set; } = [];
        public static class Factory
        {
            public static Project Create() => new Project();
        }
    }
}
