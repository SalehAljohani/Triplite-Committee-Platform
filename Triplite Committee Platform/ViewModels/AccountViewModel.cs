using System.ComponentModel.DataAnnotations;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.ViewModels
{
    public class AccountViewModel : UserModel
    {
        [Required(ErrorMessage ="notWork")]
        public IList<string?> ListRoles { get; set; }

        [StringLength(20, ErrorMessage = "passLength", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "pass")]
        public string? Password { get; set; }
    }
}
