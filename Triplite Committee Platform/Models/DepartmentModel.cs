using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class DepartmentModel
    {
        public List<BoardModel> Board { get; set; }

        [ForeignKey("CollegeNo")] public CollegeModel College { get; set; }

        [Required]
        public int CollegeNo { get; set; }

        [Required]
        [StringLength(50)]
        public string DeptName { get; set; }

        [Key]
        public int DeptNo { get; set; }

        public List<ScholarshipModel> Scholarship { get; set; }
    }
}