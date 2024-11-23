using Microsoft.AspNetCore.Mvc;
using Domain.Common.Project;
using Domain.Repositories;
using Application.Interfaces;

namespace API.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService, IUserRepository repository) : base(repository)
        {
            _projectService = projectService;
        }
        [HttpGet]
        public async Task<IActionResult> ListProjects() => Ok(await _projectService.ListProjects((await LoggedUser()).Id));
        [HttpPost]
        public async Task<IActionResult> CreateProject() => Ok(await _projectService.CreateProject((await LoggedUser()).Id));
        [HttpGet("{projectId}/task")]
        public async Task<IActionResult> ListTasks([FromRoute] Guid projectId) => Ok(await _projectService.ListTasks(projectId, (await LoggedUser()).Id));
        [HttpPost("task")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request) => Ok(await _projectService.CreateTask(request, (await LoggedUser()).Id));
        [HttpPatch("task")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequest request) => Ok(await _projectService.UpdateTask(request, (await LoggedUser()).Id));
        [HttpDelete("task/{taskId}")]
        public async Task<IActionResult> RemoveTask([FromRoute] Guid taskId) => Ok(await _projectService.RemoveTask(taskId, (await LoggedUser()).Id));
        [HttpPost("task/add-comment")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequest request) => Ok(await _projectService.AddComment(request, (await LoggedUser()).Id));
        [HttpPost("{projectId}/report")]
        public async Task<IActionResult> Report([FromBody] Guid projectId) => Ok(await _projectService.Report(projectId, (await LoggedUser()).Id));
    }
}
