using Domain.Entities;

namespace Domain.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void ShouldCreate()
        {
            var user = User.Factory.Create();
            Assert.NotNull(user);
            Assert.IsType<User>(user);
        }
        [Fact]
        public void AnUserShouldCreateAProject()
        {
            var user = User.Factory.Create();
            user.AddProject();
            Assert.NotEmpty(user.Projects);
        }
    }
}
