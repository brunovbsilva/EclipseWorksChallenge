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

        public void RemoveProject(Guid projectId)
        {
            var project = Projects.FirstOrDefault(p => p.Id == projectId);
            if (project is null) throw new ArgumentException("Project not found");
            if (project.HasPendingTask()) throw new ArgumentException("Project has pending tasks, you should finish them first");
            Projects = Projects.Where(p => p.Id != projectId);
        }
    }
}
