using Domain.Entities;
using Domain.Tests.Mocks;

namespace Domain.Tests.Entities
{
    public class LogTests
    {
        private readonly LogMock _logMock;

        public LogTests()
        {
            _logMock = new();
        }

        [Fact]
        public void ShouldCreate() {
            // Arrange & Act
            var log = Log.Factory.Create(Guid.NewGuid(), "Test", "Test", "Test");

            // Assert
            Assert.NotNull(log);
            Assert.IsType<Log>(log);
        }

        [Fact]
        public void ShouldCreateWithMock()
        {
            // Arrange & Act
            var log = _logMock.GetEntity();
            // Assert
            Assert.NotNull(log);
            Assert.IsType<Log>(log);
        }
    }
}
