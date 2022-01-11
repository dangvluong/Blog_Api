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
        public async Task<IEnumerable<Member>> GetMembers()
        {
            //return await _context.Members.Include(p => p.Roles).ToListAsync();
            return await FindAll(false).Include(member => member.Roles).ToListAsync();
        }
        //public async Task<Member> Login(LoginModel loginModel)
        //{
        //    return await _context.Members.Include(usr => usr.Roles).Where(user =>
        //       user.Username == loginModel.Username &&
        //       user.Password == SiteHelper.HashPassword(loginModel.Password)
        //     ).FirstOrDefaultAsync();
        //}
        public IQueryable<Member> GetMemberByCondition(Expression<Func<Member, bool>> expression, bool trackChanges)
        {
            //return await _context.Members.FirstOrDefaultAsync(p => p.Id == id);
            return FindByCondition(expression, trackChanges);
        }
        //public async Task<bool> CheckCurrentPasswordValid(int memberId, string password)
        //{
        //    return  await _context.Members.AnyAsync(m => m.Id == memberId && m.Password == SiteHelper.HashPassword(password));
        //}
        #region only for seed data
        public void AddRange(Member[] member)
        {
            _context.Members.AddRange(member);
        }
        #endregion
    }
}
