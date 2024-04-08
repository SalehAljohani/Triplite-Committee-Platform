using System.ComponentModel.DataAnnotations;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.ViewModels
{
    public class UserRolesViewModel : UserModel
    {
        public IList<string?> ListRoles { get; set; }

        //[EmailAddress]
        //[RegularExpression(@"^[a-zA-Z0-9._%+-]+@taibahu\.edu\.sa$", ErrorMessage = "Please enter an email from the @taibahu.edu.sa domain.")]
        //[Display(Name = "Email")]
        //public override string? Email { get; set; }

        //[RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter a 10-digit number (05XXXXXXXX).")]
        //[Display(Name = "Phone Number")]
        //public override string? PhoneNumber { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}
