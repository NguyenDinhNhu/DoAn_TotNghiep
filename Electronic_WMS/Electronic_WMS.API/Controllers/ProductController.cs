using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _iProductService;
        public ProductController(IProductService iProductService)
        {
            _iProductService = iProductService;
        }

        [HttpPost(nameof(GetList))]
        public IActionResult GetList(SearchVM search)
        {
            var result = _iProductService.GetList(search);
            return Ok(result);
        }

        [HttpGet(nameof(GetProduct))]
        public IActionResult GetProduct([FromQuery] int id)
        {
            var result = _iProductService.GetById(id);
            return Ok(result);
        }

        [HttpGet(nameof(GetListCombobox))]
        public IActionResult GetListCombobox()
        {
            var result = _iProductService.GetListCombobox();
            return Ok(result);
        }

        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromForm] InsertOrUpdateProduct product)
        {
            var result = _iProductService.Insert(product);
            return Ok(result);
        }

        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iProductService.Delete(id);
            return Ok(result);
        }

        [HttpPost(nameof(Update))]
        public IActionResult Update([FromForm] InsertOrUpdateProduct product)
        {
            var result = _iProductService.Update(product);
            return Ok(result);
        }
    }
}
