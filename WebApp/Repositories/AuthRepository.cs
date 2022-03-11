
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
            //HttpResponseMessage message = await client.PostAsJsonAsync<LoginModel>("/api/auth/login", model);
            //if (message.IsSuccessStatusCode)
            //    return await message.Content.ReadAsAsync<Member>();
            //return null;
            return await Send<LoginModel, Member>("/api/auth/login", obj,
                (client, url, obj) => client.PostAsJsonAsync<LoginModel>(url, obj)
                , message => message.Content.ReadAsAsync<Member>());
            //return (Member)response.Data;
        }
        public async Task<ResponseModel> Register(RegisterModel obj)
        {
            //return await PostJson<RegisterModel, int>("/api/auth/register", model);
            return await Send<RegisterModel>("/api/auth/register", obj, (client, url, obj) => client.PostAsJsonAsync<RegisterModel>(url, obj));
        }

        public async Task<ResponseModel> ChangePassword(ChangePasswordModel obj, string token)
        {
            return await Send("/api/auth/changepassword", obj, (client, url, obj) => client.PostAsJsonAsync(url, obj), token);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //HttpResponseMessage message = await client.PostAsJsonAsync<ChangePasswordModel>("/api/auth/changepassword", obj);
            //if (message.IsSuccessStatusCode)
            //{
            //    return new SuccessResponseModel
            //    {
            //        Status = (int)message.StatusCode
            //    };
            //}
            //try
            //{
            //    var errors =JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
            //    return errors;
            //}
            //catch (Exception)
            //{
            //    return new ErrorMessageResponseModel
            //    {
            //        Status = (int)message.StatusCode,
            //        Data = await message.Content.ReadAsStringAsync()
            //    };
            //}
        }
        public async Task<ResponseModel> ForgotPassword(ForgotPasswordModel obj)
        {
            return await Send<ForgotPasswordModel, ResetPasswordModel>("/api/auth/forgotpassword", obj, (client, url, obj) => client.PostAsJsonAsync(url, obj), message => message.Content.ReadAsAsync<ResetPasswordModel>());
            //HttpResponseMessage message = await client.PostAsJsonAsync<ForgotPasswordModel>("/api/auth/forgotpassword", obj);
            //if (message.IsSuccessStatusCode)
            //{
            //    return new SuccessResponseModel
            //    {
            //        Status = (int)message.StatusCode,
            //        Data = await message.Content.ReadAsAsync<ResetPasswordModel>()
            //    };
            //}

            //try
            //{
            //    return JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
            //}
            //catch (Exception)
            //{
            //    return new ErrorMessageResponseModel
            //    {
            //        Status = (int)message.StatusCode,
            //        Data = await message.Content.ReadAsStringAsync()
            //    };
            //}
        }

        public async Task<ResponseModel> ResetPassword(ResetPasswordModel obj)
        {
            return await Send<ResetPasswordModel>("/api/auth/resetpassword", obj,
                (client, url, obj) => client.PostAsJsonAsync<ResetPasswordModel>(url,obj)
            );
            //HttpResponseMessage message = await client.PostAsJsonAsync<ResetPasswordModel>("/api/auth/resetpassword", obj);
            //if (message.IsSuccessStatusCode)
            //{
            //    return new SuccessResponseModel
            //    {
            //        Status = (int)message.StatusCode
            //    };
            //}
            //try
            //{
            //    return JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
            //}
            //catch (Exception)
            //{
            //    return new ErrorMessageResponseModel
            //    {
            //        Status = (int)message.StatusCode,
            //        Data = await message.Content.ReadAsStringAsync()
            //    };
            //}
        }

        public async Task<ResponseModel> Logout(string token)
        {
            //return await Delete("/api/auth/logout", token);
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
