using System.Net.Http.Headers;
using WebApp.DataTransferObject;
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
            return await Post<int>("/api/member/changeavatar", content, token);
        }

        public async Task<Member> GetMemberById(int id, string token = "")
        {
            return await Get<Member>($"/api/member/{id}", token);
        }

        public async Task<int> ChangeAboutMe(ChangeAboutMeModel obj, string token)
        {
            return await PostJson<ChangeAboutMeModel,int>($"/api/member/changeaboutme", obj, token);
        }

        public async Task<List<Member>> GetMembers(string token)
        {
            return await Get<List<Member>>("/api/member", token);
        }
        public async Task<int> BanAccount(int id, string token)
        {
            return await Post<int>($"/api/member/banaccount/{id}", token: token);
        }

        public Task<int> UpdateRolesOfMember(UpdateRolesOfMemberDto obj, string token)
        {
            return PostJson<UpdateRolesOfMemberDto,int>("/api/member/updaterolesofmember", obj, token);
        }
    }
}
