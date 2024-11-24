using Application.Interfaces;
using Domain.Common.Project;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TaskController(ITaskService service, IUserRepository userRepository) : BaseController(userRepository)
    {
        private readonly ITaskService _service = service;
        [HttpPatch]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequest request) => Ok(await _service.UpdateTask(request, (await LoggedUser()).Id));
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> RemoveTask([FromRoute] Guid taskId) => Ok(await _service.RemoveTask(taskId, (await LoggedUser()).Id));
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequest request) => Ok(await _service.AddComment(request, (await LoggedUser()).Id));

    }
}
