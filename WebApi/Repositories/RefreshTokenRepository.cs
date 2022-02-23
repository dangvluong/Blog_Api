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

        public void Create(RefreshToken obj)
        {
            obj.Id = new Guid();
            Add(obj);      
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            return await FindByCondition(t => t.Token == token, trackChanges: false).FirstOrDefaultAsync();
        }
    }
}
