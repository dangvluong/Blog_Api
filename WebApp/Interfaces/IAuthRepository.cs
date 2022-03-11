using WebApp.DataTransferObject;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Interfaces
{
    public interface IAuthRepository
    {
        Task<ResponseModel> Login(LoginModel model);
        Task<ResponseModel> Register(RegisterModel model);       
        Task<ResponseModel> ChangePassword(ChangePasswordModel obj, string token);
        Task<ResponseModel> ForgotPassword(ForgotPasswordModel obj);
        Task<ResponseModel> ResetPassword(ResetPasswordModel obj);
        Task<ResponseModel> Logout(string token);
        Task<TokensDto> RefreshTokens(TokensDto refreshToken);
    }
}
