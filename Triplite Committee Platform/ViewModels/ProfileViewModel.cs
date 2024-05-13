using System.ComponentModel.DataAnnotations;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.ViewModels
{
    public class ProfileViewModel : UserModel
    {
        [Required(ErrorMessage = "Old Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string oldPassword { get; set; }

        [Required(ErrorMessage = "New Password is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("newPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string confirmPassword { get; set; }

        public CollegeModel? College { get; set; }

        public DepartmentModel? Department { get; set; }

        public string? activeRole { get; set; }
    }
}
