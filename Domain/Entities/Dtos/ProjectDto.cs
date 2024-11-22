namespace Domain.Entities.Dtos
{
    public class ProjectDto
    {
        private ProjectDto(Project project)
        {
            Id = project.Id;
            Tasks = project.Tasks.Select(t => (TaskDto)t);
        }
        public Guid Id { get; set; }
        public virtual IEnumerable<TaskDto> Tasks { get; private set; } = [];
        public static implicit operator ProjectDto(Project project) => new(project);
    }
}
