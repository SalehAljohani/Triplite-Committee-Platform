using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Triplite_Committee_Platform.Models
{
    public class UserModel : IdentityUser
    {
        [Required] public int EmployeeID { get; set; }
        [Required] public int DeptNo { get; set; }
        [ForeignKey("DeptNo")] public DepartmentModel Department { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Signature { get; set; }
    }
}
