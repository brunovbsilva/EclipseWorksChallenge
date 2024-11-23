namespace Domain.Common.Project
{
    public class ReportResponse
    {
        public int TotalTasks { get; set; }
        public int TotalCompletedTasks { get; set; }
        public int TotalPendingTasks { get; set; }
        public int TotalOverdueTasks { get; set; }
        public int TotalComments { get; set; }
        public int TasksOnLastThirtyDays { get; set; }
    }
}
