using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
        }

        public void AddToken(RefreshToken obj)
        {
            obj.Id = new Guid();
            Add(obj);      
        }

        public void DeleteToken(RefreshToken obj)
        {
            Delete(obj);
        }        

        public async Task<IEnumerable<RefreshToken>> GetByMember(int memberId, bool trackChanges)
        {
            return await FindByCondition(t => t.MemberId == memberId, trackChanges:trackChanges).ToListAsync();
        }

        public async Task<RefreshToken> GetByToken(string token, bool trackChanges)
        {
            return await FindByCondition(t => t.Token == token, trackChanges: trackChanges).FirstOrDefaultAsync();
        }

        void IRefreshTokenRepository.DeleteTokens(IEnumerable<RefreshToken> tokens)
        {
            DeleteRange(tokens);
        }
    }
}
