using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helper;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(IRepositoryManager repository) : base(repository)
        {
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            IEnumerable<Category> categories = await _repository.Category.GetCategories(trackChanges: false);
            if (categories == null)
                return NotFound();
            return Ok(categories);
        }
        [HttpGet("managergetcategories")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Category>>> ManagerGetCategories()
        {
            IEnumerable<Category> categories = await _repository.Category.ManagerGetCategories(trackChanges: false);
            if (categories == null)
                return NotFound();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            Category category = await _repository.Category.GetCategory(id, trackChanges: false);
            if (category == null)
                return NotFound("Không tìm thấy danh mục!");
            return Ok(category);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id,Category category)
        {           
            if (!ModelState.IsValid)
                return BadRequest();
            if (category.Id != id)
                return BadRequest();
            _repository.Category.UpdateCategory(category);
            await _repository.SaveChanges();
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (string.IsNullOrEmpty(category.Thumbnail))
                category.Thumbnail = DefaultValue.Thumbnail;
            _repository.Category.AddCategory(category);
            await _repository.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _repository.Category.GetCategory(id, trackChanges: true);
            if (category == null)
            {
                return NotFound("Không tìm thấy danh mục.");
            }
            _repository.Category.DeleteCategory(category);
            await _repository.SaveChanges();
            return NoContent();
        }
    }
}