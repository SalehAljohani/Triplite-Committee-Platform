using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class UserModel : IdentityUser
    {
        [ForeignKey("DeptNo")] public DepartmentModel? Department { get; set; }

        public int? DeptNo { get; set; }

        [RegularExpression("^[0-9]{10}$", ErrorMessage = "empIDLength")]
        [Display(Name = "empID")]
        public string? EmployeeID { get; set; } // it was originally int, but it should be string, to accept employeeIDs that start with 0,
                                                // also if the EmployeeId is 4000000000, it will be stored as 4000000000, but if it was int, it will be stored as 2147483647.

        [StringLength(35, ErrorMessage = "nameLength", MinimumLength = 8)]
        [RegularExpression(@"^[a-zA-Z\s\u0621-\u064A]*$", ErrorMessage = "nameCond")] // Name can be either in English or in Arabic.
        [Display(Name = "name")]
        public string? Name { get; set; }

        [StringLength(60, ErrorMessage = "signLength")]
        public string? Signature { get; set; }
    }
}