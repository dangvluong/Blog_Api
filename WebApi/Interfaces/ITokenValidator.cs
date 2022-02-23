namespace WebApi.Interfaces
{
    public interface ITokenValidator
    {
        bool ValidateRefreshToken(string refreshToken);
    }
}
