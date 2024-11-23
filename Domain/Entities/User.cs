using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        protected User() {}
        public RoleEnum Role { get; init; }
        public virtual IEnumerable<Project> Projects { get; set; } = [];
        public virtual IEnumerable<Log> Logs { get; set; } = [];
        public static class Factory
        {
            public static User Create(RoleEnum role) => new User
            {
                Role = role
            };
        }
        public Project AddProject()
        {
            var project = Project.Factory.Create(Id);
            Projects = Projects.Append(project);
            return project;
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
