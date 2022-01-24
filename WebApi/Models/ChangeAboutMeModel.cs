using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class ChangeAboutMeModel
    {
        [Required]
        public int MemberId { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string AboutMe { get; set; }
    }
}
