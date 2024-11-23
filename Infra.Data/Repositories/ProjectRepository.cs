using Domain.Entities;
using Domain.Repositories;

namespace Infra.Data.Repositories
{
    public class ProjectRepository(IUnitOfWork unitOfWork) : BaseRepository<Project>(unitOfWork), IProjectRepository
    {
    }
}
