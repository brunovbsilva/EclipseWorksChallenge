using Domain.Entities.Enums;
using Domain.Tests.Mocks;

namespace Domain.Tests.Entities
{
    public class TaskTests
    {
        private readonly Faker _faker;
        private readonly TaskMock _taskMock;
        public TaskTests()
        {
            _faker = new("pt_BR");
            _taskMock = new();

        }
        [Fact]
        public void ShouldCreate()
        {
            // Arrange & Act
            var task = Domain.Entities.Task.Factory.Create(
                _faker.Lorem.Sentence(),
                _faker.Lorem.Paragraph(),
                _faker.Date.Future(),
                _faker.Random.Enum<TaskStatusEnum>(),
                _faker.Random.Enum<PriorityEnum>()
            );

            // Assert
            Assert.NotNull(task);
            Assert.IsType<Domain.Entities.Task>(task);
        }

        [Fact]
        public void ShouldAddCommentToTask()
        {
            // Arrange
            var task = _taskMock.GetEntity();
            var comment = _faker.Lorem.Paragraph();

            // Act
            task.AddComment(comment);

            // Assert
            Assert.Contains(comment, task.Comments);
        }
    }
}
