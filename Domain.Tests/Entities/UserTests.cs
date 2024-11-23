using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Tests.Mocks;

namespace Domain.Tests.Entities
{
    public class UserTests
    {
        private readonly UserMock _userMock;
        private readonly ProjectMock _projectMock;
        private readonly TaskMock _taskMock;
        public UserTests()
        {
            _userMock = new UserMock();
            _projectMock = new ProjectMock();
            _taskMock = new TaskMock();
        }

        [Fact]
        public void ShouldCreate()
        {
            // Arrange & Act
            var user = User.Factory.Create(RoleEnum.MANAGER);

            // Assert
            Assert.NotNull(user);
            Assert.IsType<User>(user);
        }
        [Fact]
        public void ShouldCreateAProject()
        {
            var user = _userMock.GetEntity();
            user.AddProject();
            Assert.NotEmpty(user.Projects);
        }

        [Fact]
        public void ShouldRemoveProject() {
            // Arrange
            var user = _userMock.GetEntity();
            user.AddProject();
            var project = user.Projects.First();

            // Act
            user.RemoveProject(project.Id);

            // Assert
            Assert.DoesNotContain(project, user.Projects);
        }

        [Fact]
        public void ShouldThrowAnErrorWhenProjectNotFound() {
            // Arrange
            var user = _userMock.GetEntity();

            // Act
            var action = () => user.RemoveProject(Guid.NewGuid());

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ShouldThrowAnErrorWhenProjectHasPendingTasks() {
            // Arrange
            var user = _userMock.GetEntity();
            user.AddProject();
            var project = user.Projects.First();
            var task = _taskMock.GetEntity();
            var taskToAdd = project.AddTask(task.Title, task.Description, task.DueDate, task.Priority);
            taskToAdd.SetStatus(TaskStatusEnum.PENDING);

            // Act
            var action = () => user.RemoveProject(project.Id);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

    }
}
