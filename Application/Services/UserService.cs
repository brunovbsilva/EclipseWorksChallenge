using Application.Interfaces;
using Domain.Common.Project;
using Domain.Common;
using Domain.Entities.Dtos;
using Infra.Utils.Constans;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class UserService(IUserRepository userRepository, ILogRepository logRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogRepository _logRepository = logRepository;
        #region User Methods
        public async Task<BaseResponse<ProjectDto>> CreateProject(Guid _userId)
        {
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            var project = user.AddProject();
            await _userRepository.SaveChangesAsync();
            await AddLog(_userId, ProjectConstants.CREATE_PROJECT, null, (ProjectDto)project);
            return new GenericResponse<ProjectDto>(project);
        }
        public async Task<BaseResponse<IEnumerable<ProjectDto>>> ListProjects(Guid _userId)
        {
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            return new GenericResponse<IEnumerable<ProjectDto>>(user.Projects.Select(p => (ProjectDto)p));
        }
        public async Task<BaseResponse<object>> RemoveProject(Guid projectId, Guid _userId)
        {
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            var project = user.RemoveProject(projectId);
            await _userRepository.SaveChangesAsync();
            await AddLog(_userId, ProjectConstants.DELETE_PROJECT, (ProjectDto)project, null);
            return new GenericResponse<object>(null);
        }
        public async Task<BaseResponse<IEnumerable<ReportResponse>>> Report(Guid _userId)
        {
            var user = await _userRepository.GetByIDAsync(_userId);
            if (user == null) throw new ArgumentException("User not found");
            user.CheckForReport();
            return new GenericResponse<IEnumerable<ReportResponse>>(await _userRepository.GetReport());
        }
        #endregion
        #region Private Methods
        private async System.Threading.Tasks.Task AddLog(Guid _userId, string action, object? from, object? to)
            => await _logRepository.InsertWithSaveChangesAsync(Log.Factory.Create(_userId, action, from, to));
        #endregion
    }
}
