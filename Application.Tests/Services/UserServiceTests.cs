using Application.Services;
using Domain.Common;
using Domain.Common.Project;
using Domain.Entities.Dtos;
using Domain.Entities.Enums;
using Domain.Repositories;
using Domain.Tests.Mocks;

namespace Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ILogRepository> _logRepository;
        private readonly UserService _service;
        private readonly UserMock _userMock;
        private readonly ProjectMock _projectMock;
        private readonly TaskMock _taskMock;
        private readonly Faker _faker;
        public UserServiceTests() {
            _userRepository = new Mock<IUserRepository>();
            _logRepository = new Mock<ILogRepository>();
            _service = new UserService(_userRepository.Object, _logRepository.Object);
            _userMock = new UserMock();
            _projectMock = new ProjectMock();
            _taskMock = new TaskMock();
            _faker = new Faker();
        }

        #region CreateProject
        [Fact]
        public async Task CreateProject_ShouldCreateAProject()
        {
            // Arrange
            var user = _userMock.GetEntity();
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _logRepository.Setup(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()));

            // Act
            var result = await _service.CreateProject(user.Id);

            // Assert
            Assert.NotNull(result.Result);
            Assert.IsAssignableFrom<BaseResponse<ProjectDto>>(result);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Once);
        }
        [Fact]
        public async Task CreateProject_ShouldThrowAnException_WhenUserNotFound()
        {
            // Arrange
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));

            // Act
            var action = async () => await _service.CreateProject(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
            _logRepository.Verify(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Log>()), Times.Never);
        }
        #endregion
        #region ListProjects
        [Fact]
        public async Task ListProjects_ShouldListProjects()
        {
            // Arrange
            var user = _userMock.GetEntity();
            for (int i = 0; i < 5; i++) user.AddProject();
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            // Act
            var result = await _service.ListProjects(user.Id);

            // Assert
            Assert.NotNull(result.Result);
            Assert.IsAssignableFrom<BaseResponse<IEnumerable<ProjectDto>>>(result);
        }
        [Fact]
        public async Task ListProjects_ShouldThrowAnException_WhenUserNotFound()
        {
            // Arrange
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));

            // Act
            var action = async () => await _service.ListProjects(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        #endregion
        #region RemoveProject
        [Fact]
        public async Task RemoveProject_ShouldRemove()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            // Act
            var result = await _service.RemoveProject(project.Id, user.Id);

            // Assert
            Assert.Null(result.Result);
            Assert.False(result.HasErrors);
            Assert.IsAssignableFrom<BaseResponse<object>>(result);
        }
        [Fact]
        public async Task RemoveProject_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));
            // Act
            var action = async () => await _service.RemoveProject(Guid.Empty, Guid.Empty);
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        [Fact]
        public async Task RemoveProject_ShouldThrowException_WhenNoProjectFound()
        {
            // Arrange
            var user = _userMock.GetEntity();
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            // Act
            var action = async () => await _service.RemoveProject(Guid.Empty, user.Id);
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        [Fact]
        public async Task RemoveProject_ShouldThrowException_WhenProjectHasPendingTasks()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = _projectMock.GetEntity();
            var task = _taskMock.GetEntity();
            task.SetStatus(TaskStatusEnum.PENDING);
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            // Act
            var action = async () => await _service.RemoveProject(project.Id, user.Id);
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        #endregion
        #region Report
        [Fact]
        public async Task Report_ShouldReturnReport()
        {
            // Arrange
            var user = _userMock.GetEntity(RoleEnum.MANAGER);
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            // Act
            var result = await _service.Report(user.Id);
            // Assert
            Assert.NotNull(result.Result);
            Assert.IsAssignableFrom<BaseResponse<IEnumerable<ReportResponse>>>(result);
        }
        [Fact]
        public async Task Report_ShouldThrowAnException_WhenUserNotFound()
        {
            // Arrange
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));
            // Act
            var action = async () => await _service.Report(Guid.Empty);
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        [Fact]
        public async Task Report_ShouldThrowAnException_WhenUserHasNoPermissions()
        {
            // Arrange
            var user = _userMock.GetEntity(RoleEnum.WORKER);
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            // Act
            var action = async () => await _service.Report(user.Id);
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        #endregion
    }
}
