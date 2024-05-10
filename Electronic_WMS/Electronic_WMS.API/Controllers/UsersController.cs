using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _iUsersService;
        private readonly IAuthenticationService _iAuthenticationService;
        public UsersController(IUsersService iUsersService, IAuthenticationService iAuthenticationService)
        {
            _iUsersService = iUsersService;
            _iAuthenticationService = iAuthenticationService;
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(GetList))]
        public IActionResult GetList([FromBody] SearchVM search)
        {
            var result = _iUsersService.GetList(search);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetUser))]
        public IActionResult GetUser([FromQuery] int id)
        {
            var result = _iUsersService.GetById(id);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(GetListSupplierOrShop))]
        public IActionResult GetListSupplierOrShop([FromBody]int rolesId)
        {
            var result = _iUsersService.GetListSupplierOrShop(rolesId);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromForm] InsertUpdateUsers user)
        {
            var userToken = _iAuthenticationService.GetUserToken();

            if (userToken == null)
            {
                return Unauthorized("Token not found");
            }
            var result = _iUsersService.Insert(user, userToken);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iUsersService.Delete(id);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPost(nameof(Update))]
        public IActionResult Update([FromForm] InsertUpdateUsers user)
        {
            var userToken = _iAuthenticationService.GetUserToken();

            if (userToken == null)
            {
                return Unauthorized("Token not found");
            }
            var result = _iUsersService.Update(user, userToken);
            return Ok(result);
        }
    }
}
