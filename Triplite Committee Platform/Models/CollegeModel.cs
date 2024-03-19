using System.ComponentModel.DataAnnotations;

namespace Triplite_Committee_Platform.Models
{
    public class CollegeModel
    {
        [Required]
        [StringLength(50)]
        public string CollegeName { get; set; }

        [Key]
        public int CollegeNo { get; set; }
        public List<DepartmentModel> Department { get; set; }
    }
}