using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
