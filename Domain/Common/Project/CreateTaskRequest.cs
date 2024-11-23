using Domain.Entities.Enums;

namespace Domain.Common.Project
{
    public class CreateTaskRequest
    {
        public Guid ProjectId { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime DueDate { get; init; }
        public PriorityEnum Priority { get; init; }

    }
}
