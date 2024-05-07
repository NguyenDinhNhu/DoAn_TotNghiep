using Electronic_WMS.Models.Models;
using Electronic_WMS.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _iInventoryService;
        private readonly IAuthenticationService _iAuthenticationService;
        public InventoryController(IInventoryService iInventoryService, IAuthenticationService iAuthenticationService)
        {
            _iInventoryService = iInventoryService;
            _iAuthenticationService = iAuthenticationService;
        }

        [Authorize]
        [HttpPost(nameof(GetListByType))]
        public IActionResult GetListByType([FromBody] InventorySearch search)
        {
            var result = _iInventoryService.GetListByType(search);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iInventoryService.Delete(id);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] InsertOrUpdateInventory inv)
        {
            var userToken = _iAuthenticationService.GetUserToken();

            if (userToken == null)
            {
                return Unauthorized("Token not found");
            }
            var result = _iInventoryService.Insert(inv, userToken);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(Update))]
        public IActionResult Update([FromBody] InsertOrUpdateInventory inv)
        {
            var userToken = _iAuthenticationService.GetUserToken();

            if (userToken == null)
            {
                return Unauthorized("Token not found");
            }
            var result = _iInventoryService.Update(inv, userToken);
            return Ok(result);
        }

        [Authorize]
        [HttpGet(nameof(GetById))]
        public IActionResult GetById([FromQuery] int id)
        {
            var result = _iInventoryService.GetById(id);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(ChangeStatus))]
        public IActionResult ChangeStatus([FromBody] ChangeStatusInventory change)
        {
            var userToken = _iAuthenticationService.GetUserToken();

            if (userToken == null)
            {
                return Unauthorized("Token not found");
            }
            var result = _iInventoryService.ChangeStatus(change, userToken);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(ExportedPDFInventory))]
        public IActionResult ExportedPDFInventory([FromQuery] int id)
        {
            DateTime now = DateTime.Now;
            string dateTimeStr = now.ToString("yyyyMMddHHmmss");
            var result = _iInventoryService.GenerateInventoryPDF(id);
            if (result == null)
            {
                return Ok(new {StatusCode = 404, StatusMessage = "Not Found!"}); // Handle case when invoice is not found
            }

            return File(result, "application/pdf", $"invoice_{dateTimeStr}.pdf");
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(ExportExcelMoveHistory))]
        public IActionResult ExportExcelMoveHistory([FromQuery] int type)
        {
            DateTime now = DateTime.Now;
            string dateTimeStr = now.ToString("yyyyMMddHHmmss");
            var result = _iInventoryService.ExportMoveHistoryToExcel(type);

            return File(result, "application/force-download", $"move_history_{dateTimeStr}.xlsx");
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetDashBoardVM))]
        public IActionResult GetDashBoardVM()
        {
            var result = _iInventoryService.GetVMDashBoard();
            return Ok(result);
        }
    }
}
