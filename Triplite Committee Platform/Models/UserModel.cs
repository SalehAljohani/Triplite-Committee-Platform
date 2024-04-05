using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Triplite_Committee_Platform.Models
{
    public class UserModel : IdentityUser
    {
        [ForeignKey("DeptNo")] public DepartmentModel? Department { get; set; }

        public int? DeptNo { get; set; }

        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Employee ID must be a 10 numbers")]
        [Display(Name = "Employee ID")]
        public int? EmployeeID { get; set; } // must ask Dr.Fahad if employee ID is int or string

        [StringLength(35, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^[a-zA-Z\s\u0621-\u064A]*$", ErrorMessage = "The Name field should only contain letters, spaces.")] // Name can be either in English or in Arabic.
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [StringLength(60, ErrorMessage = "Signature Cannot Exceed 60 Characters.")]
        public string? Signature { get; set; }
    }
}