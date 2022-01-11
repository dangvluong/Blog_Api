﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _repository.Category.GetCategories();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            Category category  = await _repository.Category.GetCategory(id);
            if(category == null)
                return NotFound();
            return Ok(category);
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
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
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _repository.Category.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            //category.IsDeleted = true;
            _repository.Category.DeleteCategory(category);
            await _repository.SaveChanges();
            return NoContent();
        }       
    }
}
