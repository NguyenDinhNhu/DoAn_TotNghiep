using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _iRolesService;
        public RolesController(IRolesService iRolesService)
        {
            _iRolesService = iRolesService;
        }

        [HttpPost(nameof(GetList))]
        public IActionResult GetList([FromBody] SearchVM search)
        {
            var result = _iRolesService.GetList(search);
            return Ok(result);
        }

        [HttpGet(nameof(GetRoles))]
        public IActionResult GetRoles([FromQuery] int id)
        {
            var result = _iRolesService.GetById(id);
            return Ok(result);
        }

        [HttpGet(nameof(GetListCombobox))]
        public IActionResult GetListCombobox()
        {
            var result = _iRolesService.GetListCombobox();
            return Ok(result);
        }

        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] Roles role)
        {
            var result = _iRolesService.Insert(role);
            return Ok(result);
        }

        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iRolesService.Delete(id);
            return Ok(result);
        }

        [HttpPost(nameof(Update))]
        public IActionResult Update([FromBody] Roles role)
        {
            var result = _iRolesService.Update(role);
            return Ok(result);
        }
    }
}
