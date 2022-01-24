using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetMemberById(int id, string token = "");
        Task<int> ChangeAboutMe(ChangeAboutMeModel obj,string token);
        Task<int> ChangeAvatar(MultipartFormDataContent content,string token);
    }
}
