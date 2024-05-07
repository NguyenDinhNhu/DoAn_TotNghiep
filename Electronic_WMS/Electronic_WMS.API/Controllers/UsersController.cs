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
    [Authorize(Policy = "Administrator")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _iUsersService;
        private readonly IAuthenticationService _iAuthenticationService;
        public UsersController(IUsersService iUsersService, IAuthenticationService iAuthenticationService)
        {
            _iUsersService = iUsersService;
            _iAuthenticationService = iAuthenticationService;
        }

        [HttpPost(nameof(GetList))]
        public IActionResult GetList([FromBody] SearchVM search)
        {
            var result = _iUsersService.GetList(search);
            return Ok(result);
        }

        [HttpGet(nameof(GetUser))]
        public IActionResult GetUser([FromQuery] int id)
        {
            var result = _iUsersService.GetById(id);
            return Ok(result);
        }

        [HttpPost(nameof(GetListSupplierOrShop))]
        public IActionResult GetListSupplierOrShop([FromBody]int rolesId)
        {
            var result = _iUsersService.GetListSupplierOrShop(rolesId);
            return Ok(result);
        }

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

        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iUsersService.Delete(id);
            return Ok(result);
        }

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
