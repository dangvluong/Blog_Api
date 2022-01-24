using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Member
    {       
        public int Id { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string Username { get; set; }
        //[Required]        
        //public byte[] Password { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string FullName { get; set; }
        public bool Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreate { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsBanned { get; set; } = false;
        public string Token { get; set; }
        public IList<Role> Roles { get; set; }
        public string AvatarUrl { get; set; }
        public string AboutMe { get; set; }
    }
}
