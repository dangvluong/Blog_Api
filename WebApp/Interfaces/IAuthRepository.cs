using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IAuthRepository
    {
        Task<Member> Login(LoginModel model);
        Task<int> Register(RegisterModel model);
        Task<bool> CheckOldPasswordValid(string oldPassword, string token);
        Task<int> ChangePassword(string newPassword, string token);
    }
}
