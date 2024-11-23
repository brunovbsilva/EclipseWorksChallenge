namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        protected Comment() { }
        public Guid TaskId { get; private set; }
        public string Value { get; private set; }
        public virtual Task Task { get; private set; }
        public static class Factory
        {
            public static Comment Create(string value) => new Comment { Value = value };
        }

        public void UpdateTask(Task task)
        {
            Task = task;
            TaskId = task.Id;
        }
    }
}
