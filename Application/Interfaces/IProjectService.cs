using Domain.Common;
using Domain.Common.Project;
using Domain.Entities.Dtos;

namespace Application.Interfaces
{
    public interface IProjectService
    {
        Task<BaseResponse<ProjectDto>> CreateProject(Guid _userId);
        Task<BaseResponse<IEnumerable<ProjectDto>>> ListProjects(Guid _userId);
        Task<BaseResponse<IEnumerable<TaskDto>>> ListTasks(Guid projectId, Guid _userId);
        Task<BaseResponse<TaskDto>> CreateTask(CreateTaskRequest request, Guid _userId);
        Task<BaseResponse<TaskDto>> UpdateTask(UpdateTaskRequest request, Guid _userId);
        Task<BaseResponse<object>> RemoveTask(Guid taskId, Guid _userId);
        Task<BaseResponse<object>> AddComment(AddCommentRequest request, Guid _userId);
        Task<BaseResponse<ReportResponse>> Report(Guid projectId, Guid _userId);
    }
}
