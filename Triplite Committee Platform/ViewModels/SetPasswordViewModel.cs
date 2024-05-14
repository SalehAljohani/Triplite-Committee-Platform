using System.ComponentModel.DataAnnotations;

namespace Triplite_Committee_Platform.ViewModels
{
    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "passLength", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "pass")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirmPass")]
        [Compare("Password", ErrorMessage = "confirmPassMatch")]
        public string ConfirmPassword { get; set; }
    }
}
