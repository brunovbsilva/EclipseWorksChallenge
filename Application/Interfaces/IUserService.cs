using Domain.Common.Project;
using Domain.Common;
using Domain.Entities.Dtos;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<ProjectDto>> CreateProject(Guid _userId);
        Task<BaseResponse<IEnumerable<ProjectDto>>> ListProjects(Guid _userId);
        Task<BaseResponse<object>> RemoveProject(Guid projectId, Guid _userId);
        Task<BaseResponse<IEnumerable<ReportResponse>>> Report(Guid _userId);
    }
}
