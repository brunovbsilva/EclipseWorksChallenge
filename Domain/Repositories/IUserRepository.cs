using Domain.Common.Project;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<ReportResponse>> GetReport();
    }
}
