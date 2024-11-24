using Application.Interfaces;
using Domain.Common.Project;
using Domain.Common;
using Domain.Entities.Dtos;
using Domain.Entities.Enums;
using Infra.Utils.Constans;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class TaskService(ITaskRepository taskRepository, ILogRepository logRepository) : ITaskService
    {
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly ILogRepository _logRepository = logRepository;
        #region Task Methods
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
        public async Task<BaseResponse<object>> RemoveTask(Guid taskId, Guid _userId)
        {
            var task = await _taskRepository.GetByIDAsync(taskId);
            if (task == null) throw new ArgumentException("Task not found");
            task.CheckForRemove(_userId);
            await _taskRepository.DeleteAsync(task);
            await AddLog(_userId, TaskConstants.DELETE_TASK, (TaskDto)task, null);
            return new GenericResponse<object>(null);
        }
        #endregion
        #region Private Methods
        private async System.Threading.Tasks.Task AddLog(Guid _userId, string action, object? from, object? to)
            => await _logRepository.InsertWithSaveChangesAsync(Log.Factory.Create(_userId, action, from, to));
        #endregion
    }
}
