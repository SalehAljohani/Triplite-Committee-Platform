using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class ScholarshipModel
    {
        [Required]
        public DateTime Beginning_date { get; set; }

        public List<BoardModel>? Board { get; set; }

        [Required(ErrorMessage = "City Name is Required.")]
        [StringLength(50, ErrorMessage = "City Name Cannot Exceed 50 Characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country Name is Required.")]
        [StringLength(50, ErrorMessage = "Country Name Cannot Exceed 50 Characters.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Current Study is Required.")]
        [StringLength(50, ErrorMessage = "Current Study Cannot Exceed 50 Characters.")]
        public string CurrentStudy { get; set; }

        [Required(ErrorMessage = "Degree is Required.")]
        [StringLength(20, ErrorMessage = "Degree Cannot Exceed 20 Characters.")]
        public string Degree { get; set; }

        [ForeignKey("DeptNo")] public DepartmentModel? Department { get; set; }

        [Required(ErrorMessage = "Must Pick Department.")]
        public int DeptNo { get; set; }

        [Required(ErrorMessage = "Duration is Required.")]
        [StringLength(20, ErrorMessage = "Duration cannot exceed 20 characters.")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "EmployeeID is Required.")]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Gender is Required.")]
        [StringLength(10, ErrorMessage = "Gender Cannot Exceed 10 Characters.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "General Major is Required.")]
        [StringLength(50, ErrorMessage = "General Major Cannot Exceed 50 Characters.")]
        public string GeneralMajor { get; set; }

        [Required(ErrorMessage = "Name is Required.")]
        [StringLength(40, ErrorMessage = "Name Cannot Exceed 40 Characters.")]
        public string Name { get; set; }

        [Key]
        public int National_ID { get; set; }
        [Required(ErrorMessage = "Phone Number is Required.")]
        [StringLength(25, ErrorMessage = "Phone Number Cannot Exceed 25 Characters.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Specific Major is Required.")]
        [StringLength(50, ErrorMessage = "Specific Major Cannot Exceed 50 Characters.")]
        public string SpecificMajor { get; set; }

        [Required(ErrorMessage = "Status is Required.")]
        [StringLength(30, ErrorMessage = "Status Cannot Exceed 30 Characters.")]
        public string Status { get; set; }
        [Required(ErrorMessage = "University Name is Required.")]
        [StringLength(60, ErrorMessage = "University Name Cannot Exceed 60 Characters.")]
        public string University { get; set; }
    }
}