using Domain.Common.Project;
using Domain.Common;
using Domain.Entities.Dtos;

namespace Application.Interfaces
{
    public interface ITaskService
    {
        Task<BaseResponse<TaskDto>> UpdateTask(UpdateTaskRequest request, Guid _userId);
        Task<BaseResponse<object>> RemoveTask(Guid taskId, Guid _userId);
        Task<BaseResponse<object>> AddComment(AddCommentRequest request, Guid _userId);
    }
}
