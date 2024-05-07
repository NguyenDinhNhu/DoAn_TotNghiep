using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _iAuthenticationService;

        public AuthenticationController(IAuthenticationService iAuthenticationService)
        {
            _iAuthenticationService = iAuthenticationService;
        }

        [HttpPost(nameof(Login))]
        public IActionResult Login(LoginModel ul)
        {
            var token = _iAuthenticationService.Login(ul);

            if (token == null)
            {
                return BadRequest("UserName or Password is incorrect");
            }

            return Ok(new { token });
        }
    }
}
