using Domain.Entities;

namespace Domain.Tests.Mocks
{
    public class UserMock : BaseEntityMock<User>
    {
        public override User GetEntity() => User.Factory.Create();
    }
}
