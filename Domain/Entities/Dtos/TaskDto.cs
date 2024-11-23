using Domain.Entities.Enums;

namespace Domain.Entities.Dtos
{
    public class TaskDto
    {
        private TaskDto(Task task)
        {
            Id = task.Id;
            Title = task.Title;
            Description = task.Description;
            DueDate = task.DueDate;
            Status = task.Status;
            Priority = task.Priority;
            Comments = task.Comments.Select(x => x.Value);
        }
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime DueDate { get; init; }
        public TaskStatusEnum Status { get; init; }
        public PriorityEnum Priority { get; init; }
        public IEnumerable<string> Comments { get; init; } = [];

        public static implicit operator TaskDto(Task task) => new(task);

    }
}
