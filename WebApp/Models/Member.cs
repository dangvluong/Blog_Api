using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public string Email { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public bool IsBanned { get; set; }
        public string Token { get; set; }
        public List<Role> Roles { get; set; }
        public string AvatarUrl { get; set; }
        public string AboutMe { get; set; }
    }
}
