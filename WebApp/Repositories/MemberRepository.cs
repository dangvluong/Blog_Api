using System.Net.Http.Headers;

namespace WebApp.Models
{
    public class MemberRepository : BaseRepository
    {
        public MemberRepository(HttpClient client) : base(client)
        {
        }
        public async Task<int> Register(RegisterModel model)
        {
            return await Post<RegisterModel>("/api/member/register", model);            
        }
        public async Task<Member> Login(LoginModel model)
        {
            HttpResponseMessage message = await client.PostAsJsonAsync<LoginModel>("/api/member/login", model);
            if (message.IsSuccessStatusCode)
                return await message.Content.ReadAsAsync<Member>();
            return null;
        }
        public async Task<Member> GetMemberById(int id, string token)
        {
            return await Get<Member>($"/api/member/{id}", token);            
        }
        public async Task<bool> CheckOldPasswordValid(string oldPassword, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);           
            HttpResponseMessage message = await client.PostAsJsonAsync("/api/member/checkoldpasswordvalid", oldPassword);
            if (message.IsSuccessStatusCode)
                return true;
            return false;
        }
        public async Task<int> ChangePassword(string newPassword, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await client.PostAsJsonAsync("/api/member/changepassword", newPassword);
            if (message.IsSuccessStatusCode)
                return 1;
            return 0;
        }
    }
}
