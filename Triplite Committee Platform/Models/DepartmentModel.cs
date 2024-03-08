using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Triplite_Committee_Platform.Models
{
    public class DepartmentModel
    {
        [Key] public int DeptNo { get; set; }
        [Required] public string DeptName { get; set; }
        [Required] public int CollegeNo { get; set; }
        [ForeignKey("CollegeNo")] public CollegeModel College { get; set; }

        public List<BoardModel> Board { get; set; }
    }
}
