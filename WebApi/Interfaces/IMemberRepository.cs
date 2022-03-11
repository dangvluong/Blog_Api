using System.Linq.Expressions;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetMembers(bool trackChanges);
        void AddMember(Member member);
        //Task<Member> GetMember(int id);
        Task<Member> GetMemberByCondition(Expression<Func<Member, bool>> condition, bool trackChanges);
        Task<IEnumerable<Member>> GetNewMembers(bool trackChanges);
    }
}
