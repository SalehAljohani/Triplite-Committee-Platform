using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class CollegeModel
    {
        [Required(ErrorMessage = "nameReq")]
        [StringLength(50, ErrorMessage = "nameLength")]
        public string CollegeName { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollegeNo { get; set; }
        public List<DepartmentModel>? Department { get; set; }
    }
}