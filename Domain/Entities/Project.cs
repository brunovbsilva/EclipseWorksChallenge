namespace Domain.Entities
{
    public class Project : BaseEntity
    {
        protected Project() {}
        public virtual IEnumerable<Task> Tasks { get; private set; }
    }
}
