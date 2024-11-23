using Domain.Entities;

namespace Domain.Common.Project
{
    public class ReportResponse
    {
        public Guid UserId { get; set; }
        public int ActionsOnLastThirtyDays { get; set; }
        public int TasksCreatedOnLastThirtyDays { get; set; }
        public int TasksCompletedOnLastThirtyDays { get; set; }
        public int CommentsOnLastThirtyDays { get; set; }
        public int ProjectsCreatedOnLastThirtyDays { get; set; }
    }
}
