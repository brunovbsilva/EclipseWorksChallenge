using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class Project : BaseEntity
    {
        protected Project() { }
        public Guid UserId { get; private set; }
        public virtual List<Task> Tasks { get; private set; } = [];
        public virtual User User { get; private set; }
        public static class Factory
        {
            public static Project Create() => new();
        }

        public Task AddTask(Task task) 
        {
            if (Tasks.Count() == 20) throw new ArgumentException("Project has reached the maximum number of tasks");
            Tasks.Add(task);
            task.UpdateProject(this);
            return task;
        }
        public Task AddTask(string title, string description, DateTime dueDate, PriorityEnum priority)
            => AddTask(Task.Factory.Create(title, description, dueDate, priority));
        public bool HasPendingTask() => Tasks.Any(t => t.Status == TaskStatusEnum.PENDING);
        public void UpdateUser(User user)
        {
            User = user;
            UserId = user.Id;
        }
        public void CheckForListTask(Guid _userId) => CheckForBelong(_userId);
        public void CheckForCreateTask(Guid _userId) => CheckForBelong(_userId);
        private void CheckForBelong(Guid _userId)
        {
            if (UserId != _userId) throw new ArgumentException("The project do not belongs to you");
        }
    }
}
