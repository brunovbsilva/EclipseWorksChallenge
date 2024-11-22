namespace Domain.Entities
{
    public class Task : BaseEntity
    {
        protected Task() {}
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public TaskStatus Status { get; private set; }
    }
}
