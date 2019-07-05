using System.ComponentModel.DataAnnotations;

namespace Demo.Mvc.Core.Controllers.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}