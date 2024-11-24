using Application.Services;
using Domain.Common;
using Domain.Common.Project;
using Domain.Entities.Dtos;
using Domain.Entities.Enums;
using Domain.Repositories;
using Domain.Tests.Mocks;
using Infra.Data.Repositories;

namespace Application.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepository;
        private readonly Mock<ILogRepository> _logRepository;
        private readonly TaskService _service;
        private readonly UserMock _userMock;
        private readonly ProjectMock _projectMock;
        private readonly TaskMock _taskMock;
        private readonly Faker _faker;
        public TaskServiceTests()
        {
            _taskRepository = new Mock<ITaskRepository>();
            _logRepository = new Mock<ILogRepository>();
            _service = new TaskService(_taskRepository.Object, _logRepository.Object);
            _userMock = new UserMock();
            _projectMock = new ProjectMock();
            _taskMock = new TaskMock();
            _faker = new Faker();
        }
        #region RemoveTask
        [Fact]
        public async Task RemoveTask_ShouldRemoveTask()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            var taskToAdd = _taskMock.GetEntity();
            var task = project.AddTask(taskToAdd.Title, taskToAdd.Description, taskToAdd.DueDate, taskToAdd.Priority);
            _taskRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(task);
            _logRepository.Setup(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()));

            // Act
            var result = await _service.RemoveTask(task.Id, user.Id);

            // Assert
            Assert.Null(result.Result);
            Assert.IsAssignableFrom<BaseResponse<object>>(result);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Once);
        }
        [Fact]
        public async Task RemoveTask_ShouldThrowAnException_WhenTaskNotFound()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            _taskRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));

            // Act
            var action = async () => await _service.RemoveTask(Guid.Empty, user.Id);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }
        [Fact]
        public async Task RemoveTask_ShouldThrowAnException_WhenTaskDoNotBelongsToUser()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            var taskToAdd = _taskMock.GetEntity();
            var task = project.AddTask(taskToAdd.Title, taskToAdd.Description, taskToAdd.DueDate, taskToAdd.Priority);
            _taskRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(task);

            // Act
            var action = async () => await _service.RemoveTask(task.Id, Guid.NewGuid());

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }
        #endregion
        #region UpdateTask
        [Theory]
        [InlineData(true, false, false, true)]
        [InlineData(true, true, false, false)]
        [InlineData(false, true, true, false)]
        [InlineData(false, false, true, true)]
        public async Task UpdateTask_ShouldUpdateTask(bool title, bool description, bool dueDate, bool status)
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            var taskToAdd = _taskMock.GetEntity();
            var task = project.AddTask(taskToAdd.Title, taskToAdd.Description, taskToAdd.DueDate, taskToAdd.Priority);
            _taskRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(task);
            _logRepository.Setup(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()));
            var request = new UpdateTaskRequest
            {
                TaskId = task.Id,
                Title = title ? _faker.Lorem.Sentence() : null,
                Description = description ? _faker.Lorem.Paragraph() : null,
                DueDate = dueDate ? _faker.Date.Future() : null,
                Status = status ? _faker.Random.Enum<TaskStatusEnum>() : null
            };

            // Act
            var result = await _service.UpdateTask(request, user.Id);

            // Assert
            Assert.NotNull(result.Result);
            Assert.IsAssignableFrom<BaseResponse<TaskDto>>(result);
            Assert.Equal(title ? request.Title : task.Title, result.Result.Title);
            Assert.Equal(description ? request.Description : task.Description, result.Result.Description);
            Assert.Equal(dueDate ? request.DueDate : task.DueDate, result.Result.DueDate);
            Assert.Equal(status ? request.Status : task.Status, result.Result.Status);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Once);
        }
        [Fact]
        public async Task UpdateTask_ShouldThrowAnException_WhenTaskNotFound()
        {
            // Arrange
            var user = _userMock.GetEntity();
            _taskRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));

            // Act
            var action = async () => await _service.UpdateTask(new UpdateTaskRequest(), user.Id);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }
        [Fact]
        public async Task UpdateTask_ShouldThrowAnException_WhenTaskDoNotBelongsToUser()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = _projectMock.GetEntity();
            var taskToAdd = _taskMock.GetEntity();
            var task = project.AddTask(taskToAdd.Title, taskToAdd.Description, taskToAdd.DueDate, taskToAdd.Priority);
            _taskRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(task);

            // Act
            var action = async () => await _service.UpdateTask(new UpdateTaskRequest(), Guid.NewGuid());

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }
        #endregion
        #region AddComment
        [Fact]
        public async Task AddComment_ShouldAddComment()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            var taskToAdd = _taskMock.GetEntity();
            var task = project.AddTask(taskToAdd.Title, taskToAdd.Description, taskToAdd.DueDate, taskToAdd.Priority);
            _taskRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(task);
            _logRepository.Setup(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()));

            // Act
            var result = await _service.AddComment(new AddCommentRequest(), user.Id);

            // Assert
            Assert.IsAssignableFrom<BaseResponse<object>>(result);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Once);
        }
        [Fact]
        public async Task AddComment_ShouldThrowAnException_WhenTaskNotFound()
        {
            // Arrange
            _taskRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));

            // Act
            var action = async () => await _service.AddComment(new AddCommentRequest(), Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }
        [Fact]
        public async Task AddComment_ShouldThrowAnException_WhenTaskDoNotBelongsToUser()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = _projectMock.GetEntity();
            var taskToAdd = _taskMock.GetEntity();
            var task = project.AddTask(taskToAdd.Title, taskToAdd.Description, taskToAdd.DueDate, taskToAdd.Priority);
            _taskRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(task);
            _logRepository.Setup(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()));

            // Act
            var action = async () => await _service.AddComment(new AddCommentRequest(), user.Id);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }
        #endregion
    }
}
