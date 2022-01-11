using System.Linq.Expressions;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IMemberRepository 
    {
        Task<IEnumerable<Member>> GetMembers();
        void AddMember(Member member);
        //Task<Member> GetMember(int id);
        IQueryable<Member> GetMemberByCondition(Expression<Func<Member, bool>> expression, bool trackChanges);
    }
}
