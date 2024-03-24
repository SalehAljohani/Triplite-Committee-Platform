using System.ComponentModel.DataAnnotations;

namespace Triplite_Committee_Platform.Models
{
    public class CollegeModel
    {
        [Required (ErrorMessage = "College Name is Required.")]
        [StringLength(50, ErrorMessage = "College name cannot exceed 50 Characters.")]
        public string CollegeName { get; set; }

        [Key]
        public int CollegeNo { get; set; }
        public List<DepartmentModel> Department { get; set; }
    }
}