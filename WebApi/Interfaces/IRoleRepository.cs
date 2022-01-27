using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRole(int id, bool trackChanges);
        Task<IEnumerable<Role>> GetRoles(bool trackChanges);   
        void AddRole(Role role);
        void UpdateRole(Role role);
        Task<Role> GetRoleByName(string roleName, bool trackChanges);
    }
}
