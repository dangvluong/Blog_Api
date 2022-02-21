using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(IRepositoryManager repository) : base(repository)
        {
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {             
            IEnumerable<Category> categories = await _repository.Category.GetCategories(trackChanges: false);
            if (categories == null)
                return NotFound();
            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            Category category  = await _repository.Category.GetCategory(id, trackChanges: false);
            if(category == null)
                return NotFound();
            return Ok(category);
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateCategory( Category category)
        {           
            if(!ModelState.IsValid)
                return BadRequest();
            _repository.Category.UpdateCategory(category);
            await _repository.SaveChanges();
            return Ok();
        }

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Category.AddCategory(category);
            await _repository.SaveChanges();
            return Ok();
            //return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _repository.Category.GetCategory(id, trackChanges: true);
            if (category == null)
            {
                return NotFound();
            }            
            _repository.Category.DeleteCategory(category);
            await _repository.SaveChanges();
            return NoContent();
        }       
    }
}
