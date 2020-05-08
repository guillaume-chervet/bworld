

using System.ComponentModel.DataAnnotations;

namespace Demo.Mvc.Core.UserCore.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Courrier électronique")]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Provider { get; set; }
    }
}
