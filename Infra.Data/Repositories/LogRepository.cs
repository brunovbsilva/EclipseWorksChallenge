using Domain.Entities;
using Domain.Repositories;

namespace Infra.Data.Repositories
{
    public class LogRepository(IUnitOfWork unitOfWork) : BaseRepository<Log>(unitOfWork), ILogRepository
    {
    }
}
