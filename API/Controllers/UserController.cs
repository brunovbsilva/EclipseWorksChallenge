using Application.Interfaces;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController(IUserService service, IUserRepository userRepository) : BaseController(userRepository)
    {
        private readonly IUserService _service = service;
        [HttpGet]
        public async Task<IActionResult> ListProjects() => Ok(await _service.ListProjects((await LoggedUser()).Id));
        [HttpPost]
        public async Task<IActionResult> CreateProject() => Ok(await _service.CreateProject((await LoggedUser()).Id));
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> RemoveProject([FromRoute] Guid projectId) => Ok(await _service.RemoveProject(projectId, (await LoggedUser()).Id));
        [HttpGet("report")]
        public async Task<IActionResult> Report() => Ok(await _service.Report((await LoggedUser()).Id));
    }
}
