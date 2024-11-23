using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public abstract class BaseController : Controller
    {
        private readonly IUserRepository _userRepository;
        public BaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // Simulação de Microserviço de login (usualmente estaria no JWT)
        protected async Task<User> LoggedUser()
        {
            var user = _userRepository.GetAll().FirstOrDefault();
            if (user == null) { 
                user = Domain.Entities.User.Factory.Create(RoleEnum.MANAGER);
                await _userRepository.InsertWithSaveChangesAsync(user);
            }
            return user;
        }
    }
}
