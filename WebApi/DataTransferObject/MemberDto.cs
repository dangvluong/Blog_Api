using System.ComponentModel.DataAnnotations;
using WebApi.Models;

namespace WebApi.DataTransferObject
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool Gender { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsBanned { get; set; } = false;
        public string Token { get; set; }
        public string AvatarUrl { get; set; }
        public string AboutMe { get; set; }
        public List<Role> Roles { get; set; }
        public int NumberOfPost { get; set; }
        public int NumberOfComment { get; set; }
    }
}
