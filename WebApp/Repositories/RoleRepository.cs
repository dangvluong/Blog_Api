using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseModel> CreateRole(Role obj, string token)
        {            
            return await Send<Role>("/api/role", obj, (client, url, obj) => client.PostAsJsonAsync<Role>(url, obj), token);
        }

        public async Task<ResponseModel> DeleteRole(int roleId, string token)
        {           
            return await Send($"/api/role/{roleId}", (client, url) => client.DeleteAsync(url), token);
        }

        public async Task<Role> GetRoleById(int id, string token)
        {           
            var response = await Send<Role>($"/api/role/{id}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<Role>(), token);
            if (response is SuccessResponseModel)
                return (Role)response.Data;
            return null;
        }

        public async Task<IEnumerable<Role>> GetRoles(string token)
        {            
            var response = await Send<IEnumerable<Role>>("/api/role", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<IEnumerable<Role>>(), token);
            if (response is SuccessResponseModel)
                return (IEnumerable<Role>)response.Data;
            return null;
        }

        public async Task<ResponseModel> UpdateRole(Role obj, string token)
        {           
            return await Send<Role>($"/api/role/{obj.Id}", obj, (client, url, obj) => client.PutAsJsonAsync(url, obj), token);
        }
    }
}
