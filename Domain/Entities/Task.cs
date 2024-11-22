using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class Task : BaseEntity
    {
        protected Task() {}
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public TaskStatusEnum Status { get; private set; }
        public PriorityEnum Priority { get; init; }

        public static class Factory
        {
            public static Task Create(string title, string description, DateTime dueDate, TaskStatusEnum status, PriorityEnum priority)
                => new Task
                {
                    Title = title,
                    Description = description,
                    DueDate = dueDate,
                    Status = status,
                    Priority = priority
                };
        }
    }
}
