using System.ComponentModel.DataAnnotations;

namespace Triplite_Committee_Platform.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "passLength", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirmPass")]
        [Compare("Password", ErrorMessage = "confirmPassMatch")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
