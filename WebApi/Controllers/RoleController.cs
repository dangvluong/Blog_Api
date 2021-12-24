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
    public class RoleController : BaseController
    {
        public RoleController(RepositoryManager repository) : base(repository)
        {
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IEnumerable<Role>> GetRole()
        {
            return await _repository.Role.GetRoles();
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<Role> GetRole(int id)
        {
            return  await _repository.Role.GetRole(id);            
        }

        // PUT: api/Role/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdateRole(Role role)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Role.Update(role);           
            await _repository.SaveChanges();
            return Ok();
        }

        // POST: api/Role
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Role.Add(role);
            await _repository.SaveChanges();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _repository.Role.GetRole(id);
            if (role == null)
            {
                return NotFound();
            }            
            await _repository.SaveChanges();
            return NoContent();
        }        
    }
}
