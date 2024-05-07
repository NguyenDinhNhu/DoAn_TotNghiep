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
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureService _iFeatureService;
        public FeatureController(IFeatureService iFeatureService)
        {
            _iFeatureService = iFeatureService;
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(GetList))]
        public IActionResult GetList([FromBody] SearchVM search)
        {
            var result = _iFeatureService.GetList(search);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetFeature))]
        public IActionResult GetFeature([FromQuery] int id)
        {
            var result = _iFeatureService.GetById(id);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetListCombobox))]
        public IActionResult GetListCombobox()
        {
            var result = _iFeatureService.GetListCombobox();
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] Feature feature)
        {
            var result = _iFeatureService.Insert(feature);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iFeatureService.Delete(id);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPost(nameof(Update))]
        public IActionResult Update([FromBody] Feature feature)
        {
            var result = _iFeatureService.Update(feature);
            return Ok(result);
        }
    }
}
