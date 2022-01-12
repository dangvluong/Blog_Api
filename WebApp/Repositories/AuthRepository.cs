
using System.Net.Http.Headers;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        public AuthRepository(HttpClient client) : base(client)
        {
        }
        public async Task<Member> Login(LoginModel model)
        {
            HttpResponseMessage message = await client.PostAsJsonAsync<LoginModel>("/api/auth/login", model);
            if (message.IsSuccessStatusCode)
                return await message.Content.ReadAsAsync<Member>();
            return null;
        }
        public async Task<int> Register(RegisterModel model)
        {
            return await Post<RegisterModel>("/api/auth/register", model);
        }
        public async Task<bool> CheckOldPasswordValid(string oldPassword, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await client.PostAsJsonAsync("/api/auth/checkoldpasswordvalid", oldPassword);
            if (message.IsSuccessStatusCode)
                return true;
            return false;
        }
        public async Task<int> ChangePassword(string newPassword, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await client.PostAsJsonAsync("/api/auth/changepassword", newPassword);
            if (message.IsSuccessStatusCode)
                return 1;
            return 0;
        }
    }
}
