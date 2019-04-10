using System.ComponentModel.DataAnnotations;

namespace MazzaWebSite.Models
{

    public class ForgotViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}
