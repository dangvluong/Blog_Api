using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class RoleRepository : BaseRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }
        public async Task<Role> GetRole(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }
        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }
        public void Add(Role role)
        {
            _context.Roles.Add(role);
        }
        public void AddRange(Role[] roles)
        {
            _context.Roles.AddRange(roles);
        }
        public void Delete(Role role)
        {
            _context.Roles.Remove(role);
        }      
    }
}
