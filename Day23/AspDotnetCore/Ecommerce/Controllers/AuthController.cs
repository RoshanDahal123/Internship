using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public ActionResult Login()
        {
            return Ok("CreatedAt");
        }

        public ActionResult Register()
        {
            return Ok("Register endpoint");
        }
    }
}
