using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]      
        public string Email { get; set; }
    }
}
