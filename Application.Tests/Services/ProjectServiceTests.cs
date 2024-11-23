using Application.Services;
using Domain.Common;
using Domain.Entities.Dtos;
using Domain.Repositories;
using Domain.Tests.Mocks;

namespace Application.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<ITaskRepository> _taskRepository;
        private readonly UserMock _userMock;
        private readonly ProjectMock _projectMock;
        private readonly TaskMock _taskMock;
        private readonly ProjectService _service;
        public ProjectServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _projectRepository = new Mock<IProjectRepository>();
            _taskRepository = new Mock<ITaskRepository>();
            _userMock = new();
            _projectMock = new();
            _taskMock = new();
            _service = new(_userRepository.Object, _projectRepository.Object, _taskRepository.Object);

        }
        [Fact]
        public async Task CreateProject_ShouldCreateAProject()
        {
            // Arrange
            var user = _userMock.GetEntity();
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            // Act
            var result = await _service.CreateProject(user.Id);

            // Assert
            Assert.NotNull(result.Result);
            Assert.IsAssignableFrom<BaseResponse<ProjectDto>>(result);
        }
    }
}
