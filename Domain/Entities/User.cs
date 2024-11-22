namespace Domain.Entities
{
    public class User : BaseEntity
    {
        protected User() {}
        public virtual IEnumerable<Project> Projects { get; set; } = [];
        public static class Factory
        {
            public static User Create() => new User();
        }
        public void AddProject()
        {
            Projects = Projects.Append(Project.Factory.Create());
        }
    }
}
