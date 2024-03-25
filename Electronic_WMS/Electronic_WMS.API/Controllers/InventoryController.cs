using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _iInventoryService;
        public InventoryController(IInventoryService iInventoryService)
        {
            _iInventoryService = iInventoryService;
        }

        [HttpPost(nameof(GetListByType))]
        public IActionResult GetListByType([FromBody] InventorySearch search)
        {
            var result = _iInventoryService.GetListByType(search);
            return Ok(result);
        }

        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iInventoryService.Delete(id);
            return Ok(result);
        }

        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] InsertOrUpdateInventory inv)
        {
            var result = _iInventoryService.Insert(inv);
            return Ok(result);
        } 
        
        [HttpPost(nameof(Update))]
        public IActionResult Update([FromBody] InsertOrUpdateInventory inv)
        {
            var result = _iInventoryService.Update(inv);
            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        public IActionResult GetById([FromQuery] int id)
        {
            var result = _iInventoryService.GetById(id);
            return Ok(result);
        }
    }
}
