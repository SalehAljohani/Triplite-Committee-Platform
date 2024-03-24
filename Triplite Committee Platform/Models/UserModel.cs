using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Triplite_Committee_Platform.Models
{
    public class UserModel : IdentityUser
    {
        [ForeignKey("DeptNo")] public DepartmentModel Department { get; set; }

        [Required(ErrorMessage = "Must Pick Department.")]
        public int DeptNo { get; set; }

        [Required(ErrorMessage = "EmployeeID is Required.")]
        [StringLength(20, ErrorMessage = "EmployeeID Cannot Exceed 20 Characters.")]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Name is Required.")]
        [StringLength(35, ErrorMessage = "Name Cannot Exceed 35 Characters.")]
        public string Name { get; set; }

        [StringLength(60, ErrorMessage = "Signature Cannot Exceed 60 Characters.")]
        public string? Signature { get; set; }
    }
}