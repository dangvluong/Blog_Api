﻿
using Newtonsoft.Json;
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
            return await PostJson<RegisterModel>("/api/auth/register", model);
        }
       
        public async Task<BadRequestResponse> ChangePassword(ChangePasswordModel obj, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message =  await client.PostAsJsonAsync<ChangePasswordModel>("/api/auth/changepassword", obj);
            if (message.IsSuccessStatusCode)
                return null;            
            BadRequestResponse badRequest = JsonConvert.DeserializeObject<BadRequestResponse>(await message.Content.ReadAsStringAsync());
            return badRequest;                
        }
    }
}
