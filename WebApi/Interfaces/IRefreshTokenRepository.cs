using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IRefreshTokenRepository
    {
        void AddToken(RefreshToken obj);
        Task<RefreshToken> GetByToken(string token, bool trackChanges);
        void DeleteToken(RefreshToken obj);
        void DeleteTokens(IEnumerable<RefreshToken> tokens);
        Task<IEnumerable<RefreshToken>> GetByMember(int memberId, bool trackChanges);
    }
}
