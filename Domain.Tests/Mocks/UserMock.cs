using Domain.Entities;
using Domain.Entities.Enums;

namespace Domain.Tests.Mocks
{
    public class UserMock : BaseEntityMock<User>
    {
        public override User GetEntity() => User.Factory.Create(
            _faker.Random.Enum<RoleEnum>()    
        );
    }
}
