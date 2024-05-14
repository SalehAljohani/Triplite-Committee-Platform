using System.ComponentModel.DataAnnotations;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.ViewModels
{
    public class ProfileViewModel : UserModel
    {
        [Required(ErrorMessage = "oldPassReq")]
        [DataType(DataType.Password)]
        [Display(Name = "oldPass")]
        public string oldPassword { get; set; }

        [Required(ErrorMessage = "newPassReq")]
        [StringLength(20, ErrorMessage = "newPassLength", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "newPass")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "confirmPassReq")]
        [Compare("newPassword", ErrorMessage = "confirmPassMatch")]
        [DataType(DataType.Password)]
        [Display(Name = "confirmPass")]
        public string confirmPassword { get; set; }

        public CollegeModel? College { get; set; }

        public DepartmentModel? Department { get; set; }

        public string? activeRole { get; set; }
    }
}
