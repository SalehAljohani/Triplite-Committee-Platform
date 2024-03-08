using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class UserModel
    {
        [Key] public int EmployeeID { get; set; }
        [Required] public int DeptNo { get; set; }
        [ForeignKey("DeptNo")] public DepartmentModel Department { get; set; }
        [Required] public string Phone { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string Signature { get; set; }

    }
}
