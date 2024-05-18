using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class DepartmentModel
    {
        public List<BoardModel>? Board { get; set; }

        [ForeignKey("CollegeNo")] public CollegeModel? College { get; set; }

        [Required(ErrorMessage = "College Name is Required.")]
        public int CollegeNo { get; set; }

        [Required(ErrorMessage = "Department Name is Required.")]
        [StringLength(50, ErrorMessage = "Department Name Cannot Exceed 50 Characters.")]
        public string DeptName { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeptNo { get; set; }

        public List<ScholarshipModel>? Scholarship { get; set; }
    }
}