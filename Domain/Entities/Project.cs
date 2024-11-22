using Domain.Entities.Enums;

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

        public void AddTask(Task task) => Tasks = Tasks.Append(task);
        public void AddTask(string title, string description, DateTime dueDate, TaskStatusEnum status, PriorityEnum priority)
            => AddTask(Task.Factory.Create(title, description, dueDate, status, priority));
        public bool HasPendingTask() => Tasks.Any(t => t.Status == TaskStatusEnum.PENDING);
    }
}
