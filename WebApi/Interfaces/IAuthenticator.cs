using WebApi.DataTransferObject;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IAuthenticator
    {
        Task<MemberDto> Authenticate(Member member);
    }
}
