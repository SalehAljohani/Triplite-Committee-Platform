using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Triplite_Committee_Platform.Models
{
    public class DepartmentModel
    {
        [Key] public int DeptNo { get; set; }
        [Required] public string DeptName { get; set; }
        [Required] public CollegeModel College { get; set; }

    }
}
