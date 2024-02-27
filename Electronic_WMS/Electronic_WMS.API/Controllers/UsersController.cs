using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _iUsersService;
        public UsersController(IUsersService iUsersService)
        {
            _iUsersService = iUsersService;
        }

        [HttpPost(nameof(GetList))]
        public IActionResult GetList(SearchVM search)
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

        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromForm] InsertUpdateUsers user)
        {
            var result = _iUsersService.Insert(user);
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
            var result = _iUsersService.Update(user);
            return Ok(result);
        }
    }
}
