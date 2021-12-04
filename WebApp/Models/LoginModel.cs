using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
