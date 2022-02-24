using WebApp.DataTransferObject;
using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IAuthRepository
    {
        Task<Member> Login(LoginModel model);
        Task<int> Register(RegisterModel model);       
        Task<BadRequestResponse> ChangePassword(ChangePasswordModel obj, string token);
        Task<ResetPasswordModel> ForgotPassword(ForgotPasswordModel obj);
        Task<int> ResetPassword(ResetPasswordModel obj);
        Task<int> Logout(string token);
        Task<TokensDto> RefreshTokens(TokensDto refreshToken);
    }
}
