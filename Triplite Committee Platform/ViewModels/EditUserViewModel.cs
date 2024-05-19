using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.ViewModels
{
    public class EditUserViewModel : UserModel
    {
        [AllowNull] public IList<string>? ListRoles { get; set; }

        [StringLength(20, ErrorMessage = "passLength", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "pass")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirmPass")]
        [Compare("Password", ErrorMessage = "notMatch")]
        public string? ConfirmPassword { get; set; }
    }
}
