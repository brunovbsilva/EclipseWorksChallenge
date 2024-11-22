using Microsoft.AspNetCore.Mvc;
using Domain.Common.Project;

namespace API.Controllers
{
    public class ProjectController : BaseController
    {
        [HttpGet("list")]
        public IActionResult ListProjects() => Ok(true);
        [HttpPost]
        public IActionResult CreateProject() => Ok(true);
        [HttpGet("{projectId}/task")]
        public IActionResult ListTasks([FromRoute] Guid projectId) => Ok(projectId);
        [HttpPost("task")]
        public IActionResult CreateTask([FromBody] CreateTaskRequest request) => Ok(request);
        [HttpPatch("task")]
        public IActionResult UpdateTask([FromBody] UpdateTaskRequest request) => Ok(request);
        [HttpDelete("task/{taskId}")]
        public IActionResult RemoveTask([FromRoute] Guid taskId) => Ok(taskId);
    }
}
