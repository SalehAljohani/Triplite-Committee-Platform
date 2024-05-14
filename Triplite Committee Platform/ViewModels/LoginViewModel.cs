using System.ComponentModel.DataAnnotations;

namespace Triplite_Committee_Platform.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "emailOrUser")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "remember")]
        public bool RememberMe { get; set; }
    }
}
