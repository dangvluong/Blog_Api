using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRole(int id);
        Task<IEnumerable<Role>> GetRoles();   
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        Task<Role> GetRoleByName(string roleName, bool trackChanges);
    }
}
