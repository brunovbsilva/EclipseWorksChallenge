using Microsoft.AspNetCore.Mvc;
using Domain.Common.Project;
using Domain.Repositories;
using Application.Interfaces;

namespace API.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpGet("list")]
        public async Task<IActionResult> ListProjects() => Ok(await _projectService.ListProjects(_loggerUserId));
        [HttpPost]
        public async Task<IActionResult> CreateProject() => Ok(await _projectService.CreateProject(_loggerUserId));
        [HttpGet("{projectId}/task")]
        public async Task<IActionResult> ListTasks([FromRoute] Guid projectId) => Ok(await _projectService.ListTasks(projectId, _loggerUserId));
        [HttpPost("task")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request) => Ok(await _projectService.CreateTask(request, _loggerUserId));
        [HttpPatch("task")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequest request) => Ok(await _projectService.UpdateTask(request, _loggerUserId));
        [HttpDelete("task/{taskId}")]
        public async Task<IActionResult> RemoveTask([FromRoute] Guid taskId) => Ok(await _projectService.RemoveTask(taskId, _loggerUserId));
    }
}
