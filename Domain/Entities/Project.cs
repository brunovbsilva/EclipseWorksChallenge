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
            public static Project Create() => new();
        }

        public Task AddTask(Task task) 
        {
            if (Tasks.Count() == 20) throw new ArgumentException("Project has reached the maximum number of tasks");
            Tasks = Tasks.Append(task);
            task.UpdateProject(this);
            return task;
        }
        public Task AddTask(string title, string description, DateTime dueDate, PriorityEnum priority)
            => AddTask(Task.Factory.Create(title, description, dueDate, TaskStatusEnum.IN_PROGRESS, priority));
        public bool HasPendingTask() => Tasks.Any(t => t.Status == TaskStatusEnum.PENDING);
        public Task RemoveTask(Guid taskId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            Tasks = Tasks.Where(t => t.Id != taskId);
            return task;
        }
        public void UpdateUser(User user)
        {
            User = user;
            UserId = user.Id;
        }
    }
}
