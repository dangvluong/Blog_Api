using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(RepositoryManager repository) : base(repository)
        {
        }


        // GET: api/Category
        [HttpGet]
        public async Task<IList<Category>> GetCategories()
        {
            return await _repository.Category.GetCategories();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            return await _repository.Category.GetCategory(id);            
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdateCategory( Category category)
        {           
            if(!ModelState.IsValid)
                return BadRequest();
            _repository.Category.Update(category);
            await _repository.SaveChanges();
            return Ok();
        }

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Category.Add(category);
            await _repository.SaveChanges();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _repository.Category.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            category.IsDeleted = true;
            //__repository.Categories.Remove(category);
            await _repository.SaveChanges();

            return NoContent();
        }       
    }
}
