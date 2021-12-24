using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class MemberRepository : BaseRepository
    {
        public MemberRepository(AppDbContext context) : base(context)
        {
        }
        public void Add(Member member)
        {
            _context.Members.Add(member);
        }
        public void AddRange(Member[] member)
        {
            _context.Members.AddRange(member);
        }
        public async Task<IList<Member>> GetMembers()
        {
            return await _context.Members.Include(p => p.Roles).ToListAsync();
        }
        public async Task<Member> Login(LoginModel loginModel)
        {
            return await _context.Members.Include(usr => usr.Roles).Where(user =>
               user.Username == loginModel.Username &&
               user.Password == SiteHelper.HashPassword(loginModel.Password)
             ).FirstOrDefaultAsync();
        }
        public async Task<Member> GetMember(int id)
        {
            return await _context.Members.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<bool> CheckCurrentPasswordValid(int memberId, string password)
        {
            return  await _context.Members.AnyAsync(m => m.Id == memberId && m.Password == SiteHelper.HashPassword(password));
        }
    }
}
