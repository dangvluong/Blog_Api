using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRoles(string token);
        Task<Role> GetRoleById(int id, string token);
        Task<int> CreateRole(Role role, string token);
        Task<int> UpdateRole(Role role, string token);
        Task<int> DeleteRole(int roleId, string token);
    }
}
