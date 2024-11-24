using Domain.Common;
using Domain.Common.Project;
using Domain.Entities.Dtos;

namespace Application.Interfaces
{
    public interface IProjectService
    {
        Task<BaseResponse<TaskDto>> CreateTask(CreateTaskRequest request, Guid _userId);
        Task<BaseResponse<IEnumerable<TaskDto>>> ListTasks(Guid projectId, Guid _userId);
    }
}
