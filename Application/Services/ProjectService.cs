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
    public class ProjectService(IProjectRepository projectRepository, ILogRepository logRepository) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly ILogRepository _logRepository = logRepository;
        
        #region Project Methods
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
        public async Task<BaseResponse<IEnumerable<TaskDto>>> ListTasks(Guid projectId, Guid _userId)
        {
            var project = await _projectRepository.GetByIDAsync(projectId);
            if (project == null) throw new ArgumentException("Project not found");
            project.CheckForListTask(_userId);
            return new GenericResponse<IEnumerable<TaskDto>>(project.Tasks.Select(t => (TaskDto)t));
        }
        #endregion
        #region Private Methods
        private async System.Threading.Tasks.Task AddLog(Guid _userId, string action, object? from, object? to)
            => await _logRepository.InsertWithSaveChangesAsync(Log.Factory.Create(_userId, action, from, to));
        #endregion

    }
}
