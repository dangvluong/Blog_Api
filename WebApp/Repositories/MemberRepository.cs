using System.Net.Http.Headers;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class MemberRepository : BaseRepository, IMemberRepository
    {
        public MemberRepository(HttpClient client) : base(client)
        {
        }
       
        public async Task<Member> GetMemberById(int id, string token)
        {
            return await Get<Member>($"/api/member/{id}", token);            
        }        
    }
}
