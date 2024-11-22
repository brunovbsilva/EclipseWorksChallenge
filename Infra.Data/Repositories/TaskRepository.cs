using Domain.Repositories;

namespace Infra.Data.Repositories
{
    public class TaskRepository(IUnitOfWork unitOfWork) : BaseRepository<Domain.Entities.Task>(unitOfWork), ITaskRepository
    {
    }
}
