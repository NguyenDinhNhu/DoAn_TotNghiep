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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _iProductService;
        private readonly IAuthenticationService _iAuthenticationService;
        public ProductController(IProductService iProductService, IAuthenticationService iAuthenticationService)
        {
            _iProductService = iProductService;
            _iAuthenticationService = iAuthenticationService;
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(GetList))]
        public IActionResult GetList([FromBody] ProductSearch search)
        {
            var result = _iProductService.GetList(search);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(GetListProductStock))]
        public IActionResult GetListProductStock([FromBody] SearchVM search)
        {
            var result = _iProductService.GetListProductStock(search);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetProduct))]
        public IActionResult GetProduct([FromQuery] int id)
        {
            var result = _iProductService.GetById(id);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetListCombobox))]
        public IActionResult GetListCombobox()
        {
            var result = _iProductService.GetListCombobox();
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromForm] InsertOrUpdateProduct product)
        {
            var userToken = _iAuthenticationService.GetUserToken();

            if (userToken == null)
            {
                return Unauthorized("Token not found");
            }
            var result = _iProductService.Insert(product, userToken);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iProductService.Delete(id);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(Update))]
        public IActionResult Update([FromForm] InsertOrUpdateProduct product)
        {
            var userToken = _iAuthenticationService.GetUserToken();

            if (userToken == null)
            {
                return Unauthorized("Token not found");
            }
            var result = _iProductService.Update(product, userToken);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(ExportExcelStock))]
        public IActionResult ExportExcelStock()
        {
            DateTime now = DateTime.Now;
            string dateTimeStr = now.ToString("yyyyMMddHHmmss");
            var result = _iProductService.ExportStockToExcel();

            return File(result, "application/force-download", $"stock_{dateTimeStr}.xlsx");
        }
    }
}
