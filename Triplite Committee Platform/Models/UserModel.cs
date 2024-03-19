using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Triplite_Committee_Platform.Models
{
    public class UserModel : IdentityUser
    {
        [ForeignKey("DeptNo")] public DepartmentModel Department { get; set; }

        [Required]
        public int DeptNo { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(35)]
        public string Name { get; set; }

        [StringLength(60)]
        public string? Signature { get; set; }
    }
}