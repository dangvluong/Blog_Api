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

        public async Task<ResponseModel> ChangeAvatar(MultipartFormDataContent obj, string token)
        {            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await client.PostAsync("/api/member/changeavatar", obj);
            if (message.IsSuccessStatusCode)
            {
                return new SuccessResponseModel
                {
                    Status = (int)message.StatusCode
                };
            }
            try
            {
                return JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                return new ErrorMessageResponseModel
                {
                    Status = (int)message.StatusCode,
                    Data = await message.Content.ReadAsStringAsync()
                };
            }
        }

        public async Task<Member> GetMemberById(int id, string token = "")
        {
            return await Get<Member>($"/api/member/{id}", token);
        }

        public async Task<ResponseModel> ChangeAboutMe(ChangeAboutMeModel obj, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await client.PostAsJsonAsync<ChangeAboutMeModel>("/api/member/changeaboutme", obj);
            if (message.IsSuccessStatusCode)
            {
                return new SuccessResponseModel
                {
                    Status = (int)message.StatusCode
                };
            }
            try
            {
                return JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                return new ErrorMessageResponseModel
                {
                    Status = (int)message.StatusCode,
                    Data = await message.Content.ReadAsStringAsync()
                };
            }
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
