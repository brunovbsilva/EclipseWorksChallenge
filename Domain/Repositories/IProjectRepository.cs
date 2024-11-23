using Domain.Common.Project;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        public Task<ReportResponse> GetReportAsync(Guid projectId);
    }
}
