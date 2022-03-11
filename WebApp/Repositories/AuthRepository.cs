
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        public AuthRepository(HttpClient client) : base(client)
        {
        }
        public async Task<ResponseModel> Login(LoginModel obj)
        {          
            return await Send<LoginModel, Member>("/api/auth/login", obj,
                (client, url, obj) => client.PostAsJsonAsync<LoginModel>(url, obj)
                , message => message.Content.ReadAsAsync<Member>());          
        }
        public async Task<ResponseModel> Register(RegisterModel obj)
        {           
            return await Send<RegisterModel>("/api/auth/register", obj, (client, url, obj) => client.PostAsJsonAsync<RegisterModel>(url, obj));
        }

        public async Task<ResponseModel> ChangePassword(ChangePasswordModel obj, string token)
        {
            return await Send("/api/auth/changepassword", obj, (client, url, obj) => client.PostAsJsonAsync(url, obj), token);           
        }
        public async Task<ResponseModel> ForgotPassword(ForgotPasswordModel obj)
        {
            return await Send<ForgotPasswordModel, ResetPasswordModel>("/api/auth/forgotpassword", obj, (client, url, obj) => client.PostAsJsonAsync(url, obj), message => message.Content.ReadAsAsync<ResetPasswordModel>());           
        }

        public async Task<ResponseModel> ResetPassword(ResetPasswordModel obj)
        {
            return await Send<ResetPasswordModel>("/api/auth/resetpassword", obj,
                (client, url, obj) => client.PostAsJsonAsync<ResetPasswordModel>(url,obj)
            );            
        }

        public async Task<ResponseModel> Logout(string token)
        {            
            return await Send("api/auth/logout", (client, url) => client.DeleteAsync(url), token);
        }

        public async Task<TokensDto> RefreshTokens(TokensDto obj)
        {
            if (!string.IsNullOrEmpty(obj?.RefreshToken))
            {
                //return await PostJson<TokensDto, TokensDto>("/api/auth/refreshtoken", obj);
                ResponseModel response = await Send<TokensDto, TokensDto>("/api/auth/refreshtoken", obj,
                    (client, url, obj) => client.PostAsJsonAsync<TokensDto>(url, obj),
                    message => message.Content.ReadAsAsync<TokensDto>());
                if (response is SuccessResponseModel)
                    return (TokensDto)response.Data;
            }
            return null;
        }
    }
}
