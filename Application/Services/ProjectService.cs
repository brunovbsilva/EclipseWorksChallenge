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
            return new GenericResponse<ProjectDto>(user.AddProject());
        }

        public async Task<BaseResponse<TaskDto>> CreateTask(CreateTaskRequest request, Guid _userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<ProjectDto>>> ListProjects(Guid _userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<TaskDto>>> ListTasks(Guid projectId, Guid _userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<object>> RemoveTask(Guid taskId, Guid _userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<TaskDto>> UpdateTask(UpdateTaskRequest request, Guid _userId)
        {
            throw new NotImplementedException();
        }
    }
}
