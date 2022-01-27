using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Role>> GetRoles(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }
        public async Task<Role> GetRole(int id, bool trackChanges)
        {
            //return await _context.Roles.FindAsync(id);
            return await FindByCondition(role => role.Id == id, trackChanges).FirstOrDefaultAsync();
        }

        public async Task<Role> GetRoleByName(string roleName, bool trackChanges)
        {
            //return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            return await FindByCondition(role => role.Name == roleName, trackChanges).FirstOrDefaultAsync();
        }
        public void UpdateRole(Role role)
        {
            Update(role);
        }
        public void AddRole(Role role)
        {
            Add(role);
        }
        public void AddRange(Role[] roles)
        {
            _context.Roles.AddRange(roles);
        }        
    }
}
