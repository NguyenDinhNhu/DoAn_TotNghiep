using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _iBrandService;
        public BrandController(IBrandService iBrandService)
        {
            _iBrandService = iBrandService;
        }

        [HttpPost(nameof(GetList))]
        public IActionResult GetList(SearchVM search)
        {
            var result = _iBrandService.GetList(search);
            return Ok(result);
        } 
        
        [HttpGet(nameof(GetListCombobox))]
        public IActionResult GetListCombobox()
        {
            var result = _iBrandService.GetListCombobox();
            return Ok(result);
        }

        [HttpGet(nameof(GetBrand))]
        public IActionResult GetBrand([FromQuery] int id)
        {
            var result = _iBrandService.GetById(id);
            return Ok(result);
        }

        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] Brand brand)
        {
            var result = _iBrandService.Insert(brand);
            return Ok(result);
        }

        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iBrandService.Delete(id);
            return Ok(result);
        }

        [HttpPost(nameof(Update))]
        public IActionResult Update([FromBody] Brand brand)
        {
            var result = _iBrandService.Update(brand);
            return Ok(result);
        }
    }
}
