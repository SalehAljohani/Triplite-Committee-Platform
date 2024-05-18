using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class UserModel : IdentityUser
    {
        [ForeignKey("DeptNo")] public DepartmentModel? Department { get; set; }

        public int? DeptNo { get; set; }

        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Employee ID must be a 10 numbers")]
        [Display(Name = "Employee ID")]
        public string? EmployeeID { get; set; } // it was originally int, but it should be string, to accept employeeIDs that start with 0,
                                                // also if the EmployeeId is 4000000000, it will be stored as 4000000000, but if it was int, it will be stored as 2147483647.

        [StringLength(35, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^[a-zA-Z\s\u0621-\u064A]*$", ErrorMessage = "The Name field should only contain letters, spaces.")] // Name can be either in English or in Arabic.
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [StringLength(60, ErrorMessage = "Signature Cannot Exceed 60 Characters.")]
        public string? Signature { get; set; }
    }
}