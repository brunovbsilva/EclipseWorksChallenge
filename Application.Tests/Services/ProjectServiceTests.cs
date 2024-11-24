﻿using Application.Services;
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
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<ITaskRepository> _taskRepository;
        private readonly Mock<ILogRepository> _logRepository;
        private readonly UserMock _userMock;
        private readonly ProjectMock _projectMock;
        private readonly TaskMock _taskMock;
        private readonly ProjectService _service;
        private readonly Faker _faker;
        public ProjectServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _projectRepository = new Mock<IProjectRepository>();
            _taskRepository = new Mock<ITaskRepository>();
            _logRepository = new Mock<ILogRepository>();
            _userMock = new();
            _projectMock = new();
            _taskMock = new();
            _service = new(_userRepository.Object, _projectRepository.Object, _taskRepository.Object, _logRepository.Object);
            _faker = new("pt_BR");

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

        #region CreateTask
        [Fact]
        public async Task CreateTask_ShouldCreate()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _taskRepository.Setup(x => x.InsertWithSaveChangesAsync(It.IsAny<Domain.Entities.Task>()));
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
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);
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
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);
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
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);

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

        #region ListTasks
        [Fact]
        public async Task ListTasks_ShouldListTasks()
        {
            // Arrange
            var user = _userMock.GetEntity();
            var project = user.AddProject();
            for (int i = 0; i < 5; i++) project.AddTask(_taskMock.GetEntity());
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);

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
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);

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
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()));

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
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            // Act
            var action = async () => await _service.ListTasks(project.Id, user.Id);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
        #endregion

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
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);
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
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);

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
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);

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
            _projectRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(project);

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
    }
}
