using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class Project : BaseEntity
    {
        protected Project() { }
        public Guid UserId { get; private set; }
        public virtual IEnumerable<Task> Tasks { get; private set; } = [];
        public virtual User User { get; private set; }
        public static class Factory
        {
            public static Project Create(Guid userId) => new Project { UserId = userId };
        }

        public void AddTask(Task task) 
        {
            if (Tasks.Count() == 20) throw new ArgumentException("Project has reached the maximum number of tasks");
            Tasks = Tasks.Append(task);
        }
        public void AddTask(string title, string description, DateTime dueDate, TaskStatusEnum status, PriorityEnum priority)
            => AddTask(Task.Factory.Create(Id, title, description, dueDate, status, priority));
        public bool HasPendingTask() => Tasks.Any(t => t.Status == TaskStatusEnum.PENDING);
    }
}
