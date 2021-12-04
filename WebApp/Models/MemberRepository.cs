namespace WebApp.Models
{
    public class MemberRepository : BaseRepository
    {
        public MemberRepository(HttpClient client) : base(client)
        {
        }
        public async Task<int> Register(RegisterModel model)
        {
            HttpResponseMessage message = await client.PostAsJsonAsync<RegisterModel>("/api/user/register", model);
            if (message.IsSuccessStatusCode)
                return await message.Content.ReadAsAsync<int>();
            return 0;
        }
        public async Task<Member> Login(LoginModel model)
        {
            HttpResponseMessage message = await client.PostAsJsonAsync<LoginModel>("/api/user/login", model);
            if (message.IsSuccessStatusCode)
                return await message.Content.ReadAsAsync<Member>();
            return null;
        }
    }
}
