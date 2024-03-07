using System.ComponentModel.DataAnnotations;

namespace Triplite_Committee_Platform.Models
{
    public class ScholarshipModel
    {
        [Key] public int National_ID { get; set; }
        [Required] public int DeptNo { get; set; }
        [Required] public int EmployeeID { get; set; }
        [Required] public string Phone { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string CurrentStudy { get; set; }
        [Required] public string SpecificMajor { get; set; }
        [Required] public string Status { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string City { get; set; }
        [Required] public string University { get; set; }
        [Required] public string Duration { get; set; }
        [Required] public DateTime Beginning_date { get; set; }
        [Required] public string Degree { get; set; }

        //public List<BoardModel> Board { get; set; } = new List<BoardModel>();

    }
}
