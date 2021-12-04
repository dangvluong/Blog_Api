using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
