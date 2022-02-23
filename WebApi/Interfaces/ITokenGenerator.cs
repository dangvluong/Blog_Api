using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ITokenGenerator
    {
        string CreateAccessToken(Member obj);
        string CreateRefreshToken();
        string CreateResetPasswordToken(int length);
    }
}
