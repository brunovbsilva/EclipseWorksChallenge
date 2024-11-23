using Domain.Common.Project;
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
        public virtual List<Comment> Comments { get; private set; } = [];
        public virtual Project Project { get; private set; }

        public static class Factory
        {
            public static Task Create(string title, string description, DateTime dueDate, PriorityEnum priority)
                => new Task
                {
                    Title = title,
                    Description = description,
                    DueDate = dueDate,
                    Status = TaskStatusEnum.IN_PROGRESS,
                    Priority = priority
                };
        }

        public Comment AddComment(string comment)
        {
            var commentModel = Comment.Factory.Create(comment);
            commentModel.UpdateTask(this);
            Comments.Add(commentModel);
            return commentModel;
        }
        public void SetStatus(TaskStatusEnum status) => Status = status;

        public void Update(string? title, string? description, DateTime? dueDate, TaskStatusEnum? status)
        {
            Title = title ?? Title;
            Description = description ?? Description;
            DueDate = dueDate ?? DueDate;
            Status = status ?? Status;
        }
        public void UpdateProject(Project project){
            Project = project;
            ProjectId = project.Id;
        }
    }
}
