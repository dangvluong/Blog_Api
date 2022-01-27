using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(HttpClient client) : base(client)
        {
        }

        public Task<int> CreateRole(Role role, string token)
        {
            return PostJson<Role>("/api/role", role, token);
        }

        public Task<int> DeleteRole(int roleId, string token)
        {
            return Delete($"/api/role/{roleId}", token);
        }

        public Task<Role> GetRoleById(int id, string token)
        {
            return Get<Role>($"/api/role/{id}", token);
        }

        public Task<IEnumerable<Role>> GetRoles(string token)
        {
            return Get<IEnumerable<Role>>("/api/role", token);
        }

        public Task<int> UpdateRole(Role role, string token)
        {
            return Put<Role>("/api/role", role, token);
        }
    }
}
