using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRoles(string token);
        Task<Role> GetRoleById(int id, string token);
        Task<ResponseModel> CreateRole(Role role, string token);
        Task<ResponseModel> UpdateRole(Role role, string token);
        Task<ResponseModel> DeleteRole(int roleId, string token);
    }
}
