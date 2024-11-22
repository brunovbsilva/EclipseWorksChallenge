using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public abstract class BaseController : Controller
    {
        protected Guid _loggerUserId => Guid.Empty;
    }
}
