using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        public RoleController(IRepositoryManager repository) : base(repository)
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
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            Role role = await _repository.Role.GetRole(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        // PUT: api/Role/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdateRole(Role role)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Role.UpdateRole(role);           
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
            _repository.Role.AddRole(role);
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
            _repository.Role.DeleteRole(role);
            await _repository.SaveChanges();
            return NoContent();
        }        
    }
}
