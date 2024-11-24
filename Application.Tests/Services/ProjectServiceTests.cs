using Application.Services;
using Domain.Common;
using Domain.Common.Project;
using Domain.Entities.Dtos;
using Domain.Entities.Enums;
using Domain.Repositories;
using Domain.Tests.Mocks;

namespace Application.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<ILogRepository> _logRepository;
        private readonly UserMock _userMock;
        private readonly ProjectMock _projectMock;
        private readonly TaskMock _taskMock;
        private readonly ProjectService _service;
        private readonly Faker _faker;
        public ProjectServiceTests()
        {
            _projectRepository = new Mock<IProjectRepository>();
            _logRepository = new Mock<ILogRepository>();
            _userMock = new();
            _projectMock = new();
            _taskMock = new();
            _service = new(_projectRepository.Object, _logRepository.Object);
            _faker = new("pt_BR");

        }

        #region CreateTask
        [Fact]
        public async Task CreateTask_ShouldCreate()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);
            _logRepository.Setup(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()));

            // Act
            var result = await _service.CreateTask(new CreateTaskRequest(), user.Id);

            // Assert
            Assert.NotNull(result.Result);
            Assert.IsAssignableFrom<BaseResponse<TaskDto>>(result);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Once);
        }

        [Fact]
        public async Task CreateTask_ShouldThrowException_WhenProjectNotFound()
        {
            // Arrange
            var user = _userMock.GetEntity();
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));
            // Act
            var action = async () => await _service.CreateTask(GetCreateTaskMock(Guid.Empty), user.Id);
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }

        [Fact]
        public async Task CreateTask_ShouldThrowException_WhenProjectDoNotBelongsToUser()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = _projectMock.GetEntity();
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);
            // Act
            var action = async () => await _service.CreateTask(GetCreateTaskMock(project.Id), user.Id);
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }

        [Fact]
        public async Task CreateTask_ShouldThrowException_WhenMaxTasksReached()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = _projectMock.GetEntity();
            var tasks = _taskMock.GetEnumerableWithEntities(20);
            foreach (var task in tasks) project.AddTask(task);
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);

            // Act
            var action = async () => await _service.CreateTask(GetCreateTaskMock(project.Id), user.Id);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }

        private CreateTaskRequest GetCreateTaskMock(Guid projectId)
        {
            return new CreateTaskRequest
            {
                Description = _faker.Lorem.Paragraph(),
                DueDate = _faker.Date.Future(),
                Priority = _faker.Random.Enum<PriorityEnum>(),
                ProjectId = projectId,
                Title = _faker.Lorem.Sentence()
            };
        }
        #endregion

        #region ListTasks
        [Fact]
        public async Task ListTasks_ShouldListTasks()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            for (int i = 0; i < 5; i++) project.AddTask(_taskMock.GetEntity());
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);

            // Act
            var result = await _service.ListTasks(project.Id, user.Id);

            // Assert
            Assert.NotNull(result.Result);
            Assert.IsAssignableFrom<BaseResponse<IEnumerable<TaskDto>>>(result);
        }
        [Fact]
        public async Task ListTasks_ShouldThrowAnException_WhenProjectNotFound()
        {
            // Arrange
            var user = _userMock.GetEntity();
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));

            // Act
            var action = async () => await _service.ListTasks(Guid.Empty, user.Id);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        [Fact]
        public async Task ListTasks_ShouldThrowAnException_WhenUserNotFound()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);

            // Act
            var action = async () => await _service.ListTasks(project.Id, Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        [Fact]
        public async Task ListTasks_ShouldThrowAnException_WhenProjectDoNotBelongsToUser()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = _projectMock.GetEntity();
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);

            // Act
            var action = async () => await _service.ListTasks(project.Id, user.Id);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        #endregion
    }
}
