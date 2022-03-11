using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public class MemberRepository : BaseRepository, IMemberRepository
    {
        public MemberRepository(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseModel> ChangeAvatar(HttpContent obj, string token)
        {
            return await Send<HttpContent>("/api/member/changeavatar", obj, (client, url, obj) => client.PostAsync(url, obj), token);
        }

        public async Task<Member> GetMemberById(int id, string token = "")
        {
            var response = await Send<Member>($"/api/member/{id}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<Member>(), token);
            if (response is SuccessResponseModel)
                return (Member)response.Data;
            return null;
        }

        public async Task<ResponseModel> ChangeAboutMe(ChangeAboutMeModel obj, string token)
        {
            return await Send<ChangeAboutMeModel>("/api/member/changeaboutme", obj, (client, url, obj) => client.PostAsJsonAsync<ChangeAboutMeModel>(url, obj), token);
        }

        public async Task<List<Member>> GetMembers(string token)
        {
            var response = await Send<List<Member>>("/api/member", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<List<Member>>(), token);
            if (response is SuccessResponseModel)
                return (List<Member>)response.Data;
            return null;
        }
        public async Task<List<Member>> GetNewMembers(string token)
        {
            var response = await Send<List<Member>>("/api/member/getnewmembers", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<List<Member>>(), token);
            if (response is SuccessResponseModel)
                return (List<Member>)response.Data;
            return null;
        }

        public async Task<ResponseModel> BanAccount(int id, string token)
        {
            return await Send($"/api/member/banaccount/{id}", (client, url) => client.PostAsync(url, null), token);
        }

        public async Task<ResponseModel> UnbanAccount(int id, string token)
        {
            return await Send($"/api/member/unbanaccount/{id}", (client, url) => client.PostAsync(url, null), token);
        }

        public async Task<ResponseModel> UpdateRolesOfMember(UpdateRolesOfMemberDto obj, string token)
        {
            return await Send<UpdateRolesOfMemberDto>("/api/member/updaterolesofmember", obj, (client, url, obj) => client.PostAsJsonAsync<UpdateRolesOfMemberDto>(url, obj), token);
        }
    }
}
