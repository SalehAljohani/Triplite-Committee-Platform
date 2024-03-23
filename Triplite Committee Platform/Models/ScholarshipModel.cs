using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class ScholarshipModel
    {
        [Required]
        public DateTime Beginning_date { get; set; }

        public List<BoardModel> Board { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [StringLength(50)]
        public string CurrentStudy { get; set; }

        [Required]
        [StringLength(20)]
        public string Degree { get; set; }

        [ForeignKey("DeptNo")] public DepartmentModel Department { get; set; }

        [Required]
        public int DeptNo { get; set; }

        [Required]
        [StringLength(20)]
        public string Duration { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string GeneralMajor { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        [Key]
        public int National_ID { get; set; }
        [Required]
        [StringLength(25)]
        public string Phone { get; set; }
        [Required]
        [StringLength(50)]
        public string SpecificMajor { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }
        [Required]
        [StringLength(60)]
        public string University { get; set; }
    }
}