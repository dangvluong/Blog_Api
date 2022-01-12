using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetMemberById(int id, string token);
    }
}
