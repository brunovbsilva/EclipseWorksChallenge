using Domain.Entities;
using Domain.Entities.Enums;

namespace Domain.Tests.Mocks
{
    public class UserMock : BaseEntityMock<User>
    {
        public User GetEntity(RoleEnum role) => User.Factory.Create(role);
        public override User GetEntity() => GetEntity(_faker.Random.Enum<RoleEnum>());
    }
}
