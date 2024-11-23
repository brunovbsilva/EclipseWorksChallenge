using Domain.Common.Project;
using Domain.Entities;
using Domain.Repositories;

namespace Infra.Data.Repositories
{
    public class ProjectRepository(IUnitOfWork unitOfWork) : BaseRepository<Project>(unitOfWork), IProjectRepository
    {
        public Task<ReportResponse> GetReportAsync(Guid projectId)
        {
            throw new NotImplementedException();
        }
    }
}
