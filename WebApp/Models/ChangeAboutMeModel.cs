using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ChangeAboutMeModel
    {        
        public int MemberId { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string AboutMe { get; set; }
    }
}
