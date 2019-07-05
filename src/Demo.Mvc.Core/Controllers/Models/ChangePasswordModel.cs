using System.ComponentModel.DataAnnotations;

namespace Demo.Mvc.Core.Controllers.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}