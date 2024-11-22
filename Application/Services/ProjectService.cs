using Application.Interfaces;
using Domain.Common;
using Domain.Common.Project;
using Domain.Entities.Dtos;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        public async Task<BaseResponse<IEnumerable<ProjectDto>>> CreateProject(Guid _userId)
        {
            throw new NotImplementedException();
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
