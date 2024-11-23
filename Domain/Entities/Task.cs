using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class Task : BaseEntity
    {
        protected Task() { }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public TaskStatusEnum Status { get; private set; }
        public PriorityEnum Priority { get; init; }
        public Guid ProjectId { get; private set; }
        public virtual IEnumerable<Comment> Comments { get; private set; } = [];
        public virtual Project Project { get; private set; }

        public static class Factory
        {
            public static Task Create(Guid projectId, string title, string description, DateTime dueDate, TaskStatusEnum status, PriorityEnum priority)
                => new Task
                {
                    ProjectId = projectId,
                    Title = title,
                    Description = description,
                    DueDate = dueDate,
                    Status = status,
                    Priority = priority
                };
        }

        public void AddComment(string comment)
        {
            Comments = Comments.Append(Comment.Factory.Create(Id, comment));
        }
    }
}
