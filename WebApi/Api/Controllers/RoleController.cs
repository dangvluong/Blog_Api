using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            IEnumerable<Role> roles = await _repository.Role.GetRoles(trackChanges: false);
            if(roles == null)
                return NotFound();
            return Ok(roles);
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            Role role = await _repository.Role.GetRole(id,trackChanges: false);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        // PUT: api/Role/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateRole(Role role)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Role.UpdateRole(role);           
            await _repository.SaveChanges();
            return NoContent();
        }

        // POST: api/Role
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostRole(Role role)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Role.AddRole(role);
            await _repository.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _repository.Role.GetRole(id,trackChanges:true);
            if (role == null)
            {
                return NotFound();
            }
            role.IsDeleted = !role.IsDeleted;
            await _repository.SaveChanges();
            return NoContent();
        }        
    }
}
