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
    public class WareHouseController : ControllerBase
    {
        private readonly IWareHouseService _iWareHouseService;
        private readonly IAuthenticationService _iAuthenticationService;
        public WareHouseController(IWareHouseService iWareHouseService, IAuthenticationService iAuthenticationService)
        {
            _iWareHouseService = iWareHouseService;
            _iAuthenticationService = iAuthenticationService;
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(GetList))]
        public IActionResult GetList([FromBody] SearchVM search)
        {
            var result = _iWareHouseService.GetList(search);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetWareHouse))]
        public IActionResult GetWareHouse([FromQuery] int id)
        {
            var result = _iWareHouseService.GetById(id);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetListCombobox))]
        public IActionResult GetListCombobox()
        {
            var result = _iWareHouseService.GetListCombobox();
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] InsertUpdateWareHouse wh)
        {
            var userToken = _iAuthenticationService.GetUserToken();

            if (userToken == null)
            {
                return Unauthorized("Token not found");
            }
            var result = _iWareHouseService.Insert(wh, userToken);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iWareHouseService.Delete(id);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPost(nameof(Update))]
        public IActionResult Update([FromBody] InsertUpdateWareHouse wh)
        {
            var userToken = _iAuthenticationService.GetUserToken();

            if (userToken == null)
            {
                return Unauthorized("Token not found");
            }
            var result = _iWareHouseService.Update(wh, userToken);
            return Ok(result);
        }
    }
}
