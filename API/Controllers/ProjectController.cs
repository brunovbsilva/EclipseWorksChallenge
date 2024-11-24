using Microsoft.AspNetCore.Mvc;
using Domain.Common.Project;
using Domain.Repositories;
using Application.Interfaces;

namespace API.Controllers
{
    public class ProjectController(IProjectService service, IUserRepository repository) : BaseController(repository)
    {
        private readonly IProjectService _service = service;
        
        [HttpGet("{projectId}/task")]
        public async Task<IActionResult> ListTasks([FromRoute] Guid projectId) => Ok(await _service.ListTasks(projectId, (await LoggedUser()).Id));
        [HttpPost("task")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request) => Ok(await _service.CreateTask(request, (await LoggedUser()).Id));
    }
}
