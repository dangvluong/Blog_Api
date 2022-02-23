using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IRefreshTokenRepository
    {
        void Create(RefreshToken obj);
        Task<RefreshToken> GetByToken(string token);
    }
}
