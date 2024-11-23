using Domain.Entities;
using Domain.Tests.Mocks;

namespace Domain.Tests.Entities
{
    public class ProjectTests
    {
        private readonly ProjectMock _projectMock;
        private readonly TaskMock _taskMock;

        public ProjectTests()
        {
            _projectMock = new ProjectMock();
            _taskMock = new TaskMock();
        }

        [Fact]
        public void ShouldCreate()
        {
            // Arrange & Act
            var project = Project.Factory.Create(Guid.NewGuid());

            // Assert
            Assert.NotNull(project);
            Assert.IsType<Project>(project);
        }

        [Fact]
        public void ShouldAddATask()
        {
            // Arrange
            var project = _projectMock.GetEntity();
            var task = _taskMock.GetEntity();

            // Act
            project.AddTask(task);

            // Assert
            Assert.Contains(task, project.Tasks);
        }

        [Fact]
        public void ShouldNotAddTaskIfAlreadyReachedLimit()
        {
            // Arrange
            var project = _projectMock.GetEntity();
            var tasks = _taskMock.GetEnumerableWithEntities(20);
            foreach (var task in tasks) project.AddTask(task);
            var taskToAdd = _taskMock.GetEntity();

            // Act
            var action = () => project.AddTask(taskToAdd);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}
