using Domain.Entities;
using Domain.Repositories;

namespace Infra.Data.Repositories
{
    public class UserRepository(IUnitOfWork unitOfWork) : BaseRepository<User>(unitOfWork), IUserRepository
    {
    }
}
