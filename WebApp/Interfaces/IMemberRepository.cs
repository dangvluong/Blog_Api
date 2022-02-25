using WebApp.DataTransferObject;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetMemberById(int id, string token = "");
        Task<ResponseModel> ChangeAboutMe(ChangeAboutMeModel obj,string token);
        Task<ResponseModel> ChangeAvatar(MultipartFormDataContent content,string token);
        Task<List<Member>> GetMembers(string token);
        Task<int> BanAccount(int id, string token);
        Task<int> UpdateRolesOfMember(UpdateRolesOfMemberDto obj, string token);
    }
}
