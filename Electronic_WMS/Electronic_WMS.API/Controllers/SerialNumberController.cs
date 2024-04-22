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
        
        [HttpPost(nameof(GetListSerialCombobox))]
        public IActionResult GetListSerialCombobox([FromBody] SearchListSerialCombobox search)
        {
            var result = _iSerialNumberService.GetListSerialCombobox(search);
            return Ok(result);
        }

        [HttpPost(nameof(UpdateLocation))]
        public IActionResult UpdateLocation([FromBody] List<UpdateLocation> listSeri)
        {
            var result = _iSerialNumberService.UpdateLocation(listSeri);
            return Ok(result);
        }
    }
}
