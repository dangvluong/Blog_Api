using WebApp.DataTransferObject;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetMemberById(int id, string token = "");
        Task<ResponseModel> ChangeAboutMe(ChangeAboutMeModel obj, string token);
        Task<ResponseModel> ChangeAvatar(HttpContent content, string token);
        Task<List<Member>> GetMembers(string token);
        Task<ResponseModel> BanAccount(int id, string token);
        Task<ResponseModel> UpdateRolesOfMember(UpdateRolesOfMemberDto obj, string token);
        Task<ResponseModel> UnbanAccount(int id, string accessToken);
        Task<List<Member>> GetNewMembers(string accessToken);
    }
}
