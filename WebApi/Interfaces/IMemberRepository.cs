using System.Linq.Expressions;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetMembers(bool trackChanges);
        void AddMember(Member member);
        //Task<Member> GetMember(int id);
        Task<Member> GetMemberByCondition(Expression<Func<Member, bool>> condition, bool trackChanges);
        Task<List<Member>> GetNewMembers(bool trackChanges);
    }
}
