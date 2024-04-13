using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialNumberController : Controller
    {
        private readonly ISerialNumberService _iSerialNumberService;
        public SerialNumberController(ISerialNumberService iSerialNumberService)
        {
            _iSerialNumberService = iSerialNumberService;
        }

        [HttpPost(nameof(GetListByProductId))]
        public IActionResult GetListByProductId([FromBody] SearchSeriVM search)
        {
            var result = _iSerialNumberService.GetListByProductId(search);
            return Ok(result);
        }

        [HttpPost(nameof(Updatelocation))]
        public IActionResult Updatelocation([FromBody] UpdateLocation location)
        {
            var result = _iSerialNumberService.UpdateLocation(location);
            return Ok(result);
        }
    }
}
