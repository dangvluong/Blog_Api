
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
        public async Task<Member> Login(LoginModel model)
        {
            HttpResponseMessage message = await client.PostAsJsonAsync<LoginModel>("/api/auth/login", model);
            if (message.IsSuccessStatusCode)
                return await message.Content.ReadAsAsync<Member>();
            return null;
        }
        public async Task<int> Register(RegisterModel model)
        {
            return await PostJson<RegisterModel, int>("/api/auth/register", model);
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
            return await Send<ForgotPasswordModel, ResetPasswordModel>("/api/auth/forgotpassword", obj, (client, url, obj) => client.PostAsJsonAsync(url, obj),message => message.Content.ReadAsAsync<ResetPasswordModel>()); 
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
            HttpResponseMessage message = await client.PostAsJsonAsync<ResetPasswordModel>("/api/auth/resetpassword", obj);
            if (message.IsSuccessStatusCode)
            {
                return new SuccessResponseModel
                {
                    Status = (int)message.StatusCode
                };
            }
            try
            {
                return JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                return new ErrorMessageResponseModel
                {
                    Status = (int)message.StatusCode,
                    Data = await message.Content.ReadAsStringAsync()
                };
            }
        }

        public async Task<int> Logout(string token)
        {
            return await Delete("/api/auth/logout", token);
        }

        public async Task<TokensDto> RefreshTokens(TokensDto refreshToken)
        {
            if (!string.IsNullOrEmpty(refreshToken?.RefreshToken))
            {
                return await PostJson<TokensDto, TokensDto>("/api/auth/refreshtoken", refreshToken);
            }
            return null;
        }
    }
}
