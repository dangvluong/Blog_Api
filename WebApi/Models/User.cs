using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(32, MinimumLength =6)]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateOnly DateCreate { get; set; }
    }
}
