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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _iCategoryService;
        public CategoryController(ICategoryService iCategoryService)
        {
            _iCategoryService = iCategoryService;
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpPost(nameof(GetList))]
        public IActionResult GetList([FromBody] SearchVM search) 
        {
            var result = _iCategoryService.GetList(search);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetCategory))]
        public IActionResult GetCategory([FromQuery] int id)
        {
            var result = _iCategoryService.GetById(id);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetListCombobox))]
        public IActionResult GetListCombobox()
        {
            var result = _iCategoryService.GetListCombobox();
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrStocker")]
        [HttpGet(nameof(GetCategoryParentCombobox))]
        public IActionResult GetCategoryParentCombobox()
        {
            var result = _iCategoryService.GetCategoryParentCombobox();
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] Category category)
        {
            var result = _iCategoryService.Insert(category);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPatch(nameof(Delete))]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _iCategoryService.Delete(id);
            return Ok(result);
        }

        [Authorize(Policy = "Administrator")]
        [HttpPost(nameof(Update))]
        public IActionResult Update([FromBody] Category cate)
        {
            var result = _iCategoryService.Update(cate);
            return Ok(result);
        }
    }
}
