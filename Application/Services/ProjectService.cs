using Application.Interfaces;
using Domain.Common;
using Domain.Common.Project;
using Domain.Entities;
using Domain.Entities.Dtos;
using Domain.Entities.Enums;
using Domain.Repositories;
using Infra.Utils.Constans;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogRepository _logRepository;

        public ProjectService(IUserRepository userRepository, IProjectRepository projectRepository, ITaskRepository taskRepository, ILogRepository logRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _logRepository = logRepository;
        }

        public async Task<BaseResponse<ProjectDto>> CreateProject(Guid _userId)
        {
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            var project = user.AddProject();
            await _userRepository.SaveChangesAsync();
            await AddLog(_userId, ProjectConstants.CREATE_PROJECT, null, (ProjectDto)project);
            return new GenericResponse<ProjectDto>(project);
        }

        public async Task<BaseResponse<TaskDto>> CreateTask(CreateTaskRequest request, Guid _userId)
        {
            var project = await _projectRepository.GetByIDAsync(request.ProjectId);
            if (project == null) throw new ArgumentException("Project not found");
            project.CheckForCreateTask(_userId);
            var task = project.AddTask(request.Title, request.Description, request.DueDate, request.Priority);
            await _projectRepository.SaveChangesAsync();
            await AddLog(_userId, TaskConstants.CREATE_TASK, null, (TaskDto)task);
            return new GenericResponse<TaskDto>(task);
        }

        public async Task<BaseResponse<IEnumerable<ProjectDto>>> ListProjects(Guid _userId)
        {
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            return new GenericResponse<IEnumerable<ProjectDto>>(user.Projects.Select(p => (ProjectDto)p));
        }

        public async Task<BaseResponse<IEnumerable<TaskDto>>> ListTasks(Guid projectId, Guid _userId)
        {
            var project = await _projectRepository.GetByIDAsync(projectId);
            if (project == null) throw new ArgumentException("Project not found");
            project.CheckForListTask(_userId);
            return new GenericResponse<IEnumerable<TaskDto>>(project.Tasks.Select(t => (TaskDto)t));
        }

        public async Task<BaseResponse<object>> RemoveTask(Guid taskId, Guid _userId)
        {
            var task = await _taskRepository.GetByIDAsync(taskId);
            if (task == null) throw new ArgumentException("Task not found");
            task.CheckForRemove(_userId);
            await AddLog(_userId, TaskConstants.DELETE_TASK, (TaskDto)task, null);
            await _taskRepository.DeleteAsync(task);
            return new GenericResponse<object>(null);
        }

        public async Task<BaseResponse<TaskDto>> UpdateTask(UpdateTaskRequest request, Guid _userId)
        {
            var task = await _taskRepository.GetByIDAsync(request.TaskId);
            if (task == null) throw new ArgumentException("Task not found");
            task.CheckForUpdate(_userId);
            var taskBefore = (TaskDto)task;
            task.Update(request.Title, request.Description, request.DueDate, request.Status);
            await _taskRepository.SaveChangesAsync();
            await AddLog(
                _userId, 
                request.Status == TaskStatusEnum.DONE ? TaskConstants.COMPLETE_TASK : TaskConstants.UPDATE_TASK,
                taskBefore,
                (TaskDto)task
            );
            return new GenericResponse<TaskDto>(task);
        }

        public async Task<BaseResponse<object>> AddComment(AddCommentRequest request, Guid _userId)
        {
            var task = await _taskRepository.GetByIDAsync(request.TaskId);
            if (task == null) throw new ArgumentException("Task not found");
            task.CheckForComment(_userId);
            var commentsBefore = task.Comments.Any() ? task.Comments.Select(x => x.Value).ToList() : null;
            task.AddComment(request.Comment);
            await _taskRepository.SaveChangesAsync();
            await AddLog(_userId, TaskConstants.ADD_COMMENT, commentsBefore, task.Comments.Select(x => x.Value));
            return new GenericResponse<object>(null);
        }
        public async Task<BaseResponse<IEnumerable<ReportResponse>>> Report(Guid _userId)
        {
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            user.CheckForReport();
            return new GenericResponse<IEnumerable<ReportResponse>>(await _userRepository.GetReport());
        }
        private async System.Threading.Tasks.Task AddLog(Guid _userId, string action, object? from, object? to)
            => await _logRepository.InsertWithSaveChangesAsync(Log.Factory.Create(_userId, action, from, to));

    }
}
