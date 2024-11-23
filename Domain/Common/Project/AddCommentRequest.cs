namespace Domain.Common.Project
{
    public class AddCommentRequest
    {
        public Guid TaskId { get; set; }
        public string Comment { get; set; }
    }
}
