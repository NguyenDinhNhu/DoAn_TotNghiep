using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHouseController : ControllerBase
    {
        private readonly IWareHouseService _iWareHouseService;
        public WareHouseController(IWareHouseService iWareHouseService)
        {
            _iWareHouseService = iWareHouseService;
        }

        [HttpPost(nameof(GetList))]
        public IActionResult GetList(SearchVM search)
        {
            var result = _iWareHouseService.GetList(search);
            return Ok(result);
        }

        [HttpGet(nameof(GetWareHouse))]
        public IActionResult GetWareHouse([FromQuery] int id)
        {
            var result = _iWareHouseService.GetById(id);
            return Ok(result);
        }

        [HttpGet(nameof(GetListCombobox))]
        public IActionResult GetListCombobox()
        {
            var result = _iWareHouseService.GetListCombobox();
            return Ok(result);
        }

        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] InsertUpdateWareHouse wh)
        {
            var result = _iWareHouseService.Insert(wh);
            return Ok(result);
        }

        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iWareHouseService.Delete(id);
            return Ok(result);
        }

        [HttpPost(nameof(Update))]
        public IActionResult Update([FromBody] InsertUpdateWareHouse wh)
        {
            var result = _iWareHouseService.Update(wh);
            return Ok(result);
        }
    }
}
