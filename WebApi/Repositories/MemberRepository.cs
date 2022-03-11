using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class MemberRepository : BaseRepository<Member>, IMemberRepository
    {
        public MemberRepository(AppDbContext context) : base(context)
        {
        }
        public void AddMember(Member member)
        {
            _context.Members.Add(member);
        }      
        public async Task<List<Member>> GetMembers(bool trackChanges)
        {           
            return await FindAll(trackChanges).Include(member => member.Roles).ToListAsync();
        }
        
        public async Task<Member> GetMemberByCondition(Expression<Func<Member, bool>> condition, bool trackChanges)
        {            
            return await FindByCondition(condition, trackChanges).Include(m => m.Roles).FirstOrDefaultAsync();
        }
        
        public void AddRange(Member[] member)
        {
            _context.Members.AddRange(member);
        }

        public async Task<List<Member>> GetNewMembers(bool trackChanges)
        {
            return await FindByCondition(m => m.DateCreate >= DateTime.Now.AddDays(-30),trackChanges).Include(m => m.Roles).ToListAsync();
        }
    }
}
