using Domain.Entities.Enums;

namespace Domain.Common.Project
{
    public class UpdateTaskRequest
    {
        public Guid TaskId { get; init; }
        public string? Title { get; init; }
        public string? Description { get; init; }
        public DateTime? DueDate { get; init; }
        public TaskStatusEnum? Status { get; init; }
    }
}
