namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        protected Comment() { }
        public Guid TaskId { get; init; }
        public string Value { get; private set; }
        public virtual Task Task { get; init; }
        public static class Factory
        {
            public static Comment Create(Guid taskId, string value) => new Comment { TaskId = taskId, Value = value };
        }
    }
}
