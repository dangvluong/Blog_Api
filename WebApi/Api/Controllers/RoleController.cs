using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        public RoleController(IRepositoryManager repository) : base(repository)
        {
        }       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            IEnumerable<Role> roles = await _repository.Role.GetRoles(trackChanges: false);
            if(roles == null)
                return NotFound();
            return Ok(roles);
        }       
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            Role role = await _repository.Role.GetRole(id,trackChanges: false);
            if (role == null)
                return NotFound();
            return Ok(role);
        }      
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
