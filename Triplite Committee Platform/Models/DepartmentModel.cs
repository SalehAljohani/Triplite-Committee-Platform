using System.ComponentModel.DataAnnotations;
namespace Triplite_Committee_Platform.Models
{
    public class DepartmentModel
    {
        [Key] public int DeptNo { get; set; }
        [Required] public string DeptName { get; set; }
    }
}
