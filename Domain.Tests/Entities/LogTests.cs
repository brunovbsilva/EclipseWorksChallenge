using Domain.Entities;

namespace Domain.Tests.Entities
{
    public class LogTests
    {
        [Fact]
        public void ShouldCreate() {
            // Arrange & Act
            var log = Log.Factory.Create("Test", "Test", "Test");

            // Assert
            Assert.NotNull(log);
            Assert.IsType<Log>(log);
        }
    }
}
