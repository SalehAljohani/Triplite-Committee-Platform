using System.ComponentModel.DataAnnotations;
namespace Triplite_Committee_Platform.Models
{
    public class CollegeModel
    {
        [Key] public int CollegeNo { get; set; }
        [Required] public string CollegeName { get; set; }

        public List<DepartmentModel> Department { get; set; }
    }
}
