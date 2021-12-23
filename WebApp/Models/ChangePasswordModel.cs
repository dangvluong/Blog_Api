using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [StringLength(64, MinimumLength =6)]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string NewPassword { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
