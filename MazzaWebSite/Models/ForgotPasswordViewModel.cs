using System.ComponentModel.DataAnnotations;

namespace MazzaWebSite.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}