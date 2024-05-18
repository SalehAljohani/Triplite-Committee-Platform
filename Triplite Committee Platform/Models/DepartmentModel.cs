using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class DepartmentModel
    {
        public List<BoardModel>? Board { get; set; }

        [ForeignKey("CollegeNo")] public CollegeModel? College { get; set; }

        [Required(ErrorMessage = "collegeReq")]
        public int CollegeNo { get; set; }

        [Required(ErrorMessage = "deptReq")]
        [StringLength(50, ErrorMessage = "deptLength")]
        public string DeptName { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeptNo { get; set; }

        public List<ScholarshipModel>? Scholarship { get; set; }
    }
}