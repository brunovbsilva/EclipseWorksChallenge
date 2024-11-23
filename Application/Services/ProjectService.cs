using Application.Interfaces;
using Domain.Common;
using Domain.Common.Project;
using Domain.Entities.Dtos;
using Domain.Repositories;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        public ProjectService(IUserRepository userRepository, IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }
        public async Task<BaseResponse<ProjectDto>> CreateProject(Guid _userId)
        {
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            var project = user.AddProject();
            await _userRepository.SaveChangesAsync();
            return new GenericResponse<ProjectDto>();
        }

        public async Task<BaseResponse<TaskDto>> CreateTask(CreateTaskRequest request, Guid _userId)
        {
            var project = await _projectRepository.GetByIDAsync(request.ProjectId);
            if (project == null) throw new ArgumentException("Project not found");
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            if (!user.Projects.Contains(project)) throw new ArgumentException("The project do not belongs to you");
            var task = project.AddTask(request.Title, request.Description, request.DueDate, request.Priority);
            await _taskRepository.InsertWithSaveChangesAsync(task);
            return new GenericResponse<TaskDto>(task);
        }

        public async Task<BaseResponse<IEnumerable<ProjectDto>>> ListProjects(Guid _userId)
        {
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            return new GenericResponse<IEnumerable<ProjectDto>>(user.Projects.Select(p => (ProjectDto)p));
        }

        public async Task<BaseResponse<IEnumerable<TaskDto>>> ListTasks(Guid projectId, Guid _userId)
        {
            var project = await _projectRepository.GetByIDAsync(projectId);
            if (project == null) throw new ArgumentException("Project not found");
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            if (!user.Projects.Contains(project)) throw new ArgumentException("The project do not belongs to you");
            return new GenericResponse<IEnumerable<TaskDto>>(project.Tasks.Select(t => (TaskDto)t));
        }

        public async Task<BaseResponse<object>> RemoveTask(Guid taskId, Guid _userId)
        {
            var task = await _taskRepository.GetByIDAsync(taskId);
            if (task == null) throw new ArgumentException("Task not found");
            var project = await _projectRepository.GetByIDAsync(task.ProjectId);
            if (project!.UserId != _userId) throw new ArgumentException("The task do not belongs to you");
            project.RemoveTask(task.Id);
            await _taskRepository.SaveChangesAsync();
            return new GenericResponse<object>(null);
        }

        public async Task<BaseResponse<TaskDto>> UpdateTask(UpdateTaskRequest request, Guid _userId)
        {
            var task = await _taskRepository.GetByIDAsync(request.TaskId);
            if (task == null) throw new ArgumentException("Task not found");
            if (task.Project.UserId != _userId) throw new ArgumentException("The task do not belongs to you");
            task.Update(request.Title, request.Description, request.DueDate);
            await _taskRepository.SaveChangesAsync();
            return new GenericResponse<TaskDto>(task);
        }
    }
}
