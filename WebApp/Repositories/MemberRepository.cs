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

        public async Task<int> ChangeAvatar(MultipartFormDataContent content, string token)
        {
            return await Post("/api/member/changeavatar", content, token);
        }

        public async Task<Member> GetMemberById(int id, string token = "")
        {
            return await Get<Member>($"/api/member/{id}", token);
        }

        public async Task<int> ChangeAboutMe(ChangeAboutMeModel obj, string token)
        {
            return await PostJson<ChangeAboutMeModel>($"/api/member/changeaboutme", obj, token);
        }
    }
}
