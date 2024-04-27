using System.ComponentModel.DataAnnotations;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.ViewModels
{
    public class AccountViewModel : UserModel
    {
        public IList<string?> ListRoles { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}
