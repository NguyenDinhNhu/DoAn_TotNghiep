using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductFeatureController : ControllerBase
    {
        private readonly IProductFeatureService _iProductFeatureService;
        public ProductFeatureController(IProductFeatureService iProductFeatureService)
        {
            _iProductFeatureService = iProductFeatureService;
        }

        [HttpPost(nameof(GetList))]
        public IActionResult GetList(SearchVM search)
        {
            var result = _iProductFeatureService.GetList(search);
            return Ok(result);
        }

        [HttpGet(nameof(GetListProductFeature))]
        public IActionResult GetListProductFeature([FromQuery] int productId)
        {
            var result = _iProductFeatureService.GetListByProductId(productId);
            return Ok(result);
        }
        [HttpGet(nameof(GetPFById))]
        public IActionResult GetPFById([FromQuery] int id)
        {
            var result = _iProductFeatureService.GetById(id);
            return Ok(result);
        }

        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] List<ProductFeature> listPF)
        {
            var result = _iProductFeatureService.Insert(listPF);
            return Ok(result);
        }

        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iProductFeatureService.Delete(id);
            return Ok(result);
        }

        [HttpPost(nameof(Update))]
        public IActionResult Update([FromBody] ProductFeature pf)
        {
            var result = _iProductFeatureService.Update(pf);
            return Ok(result);
        }
    }
}
