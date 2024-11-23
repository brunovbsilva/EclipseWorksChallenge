using Domain.Common.Project;
using Domain.Entities;
using Domain.Repositories;
using Infra.Utils.Constans;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class UserRepository(IUnitOfWork unitOfWork) : BaseRepository<User>(unitOfWork), IUserRepository
    {
        public async Task<IEnumerable<ReportResponse>> GetReport()
        {
            return await GetAll().Select(x => new ReportResponse
            {
                UserId = x.Id,
                ActionsOnLastThirtyDays = x.Logs.Count,
                TasksCreatedOnLastThirtyDays = x.Logs.Where(y => y.Type == TaskConstants.CREATE_TASK).Count(),
                TasksCompletedOnLastThirtyDays = x.Logs.Where(y => y.Type == TaskConstants.COMPLETE_TASK).Count(),
                CommentsOnLastThirtyDays = x.Logs.Where(y => y.Type == TaskConstants.ADD_COMMENT).Count(),
                ProjectsCreatedOnLastThirtyDays = x.Logs.Where(y => y.Type == ProjectConstants.CREATE_PROJECT).Count()
            }).ToListAsync();
        }
    }
}
