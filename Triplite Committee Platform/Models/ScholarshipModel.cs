using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class ScholarshipModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="National ID is required")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "National ID must be a 10 numbers")]
        public string National_ID { get; set; }

        [ForeignKey("DeptNo")] public DepartmentModel? Department { get; set; }

        [Required(ErrorMessage = "pickDept")]
        public int DeptNo { get; set; }

        [Required(ErrorMessage = "Employee ID is Required.")]
        [StringLength(10, ErrorMessage = "Employee ID Cannot Exceed 10 Characters.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Employee ID must be a 10 numbers")]
        public string EmployeeID { get; set; }

        [Required(ErrorMessage = "phoneReq")]
        [StringLength(25, ErrorMessage = "phoneLength")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "emailReq")]
        [StringLength(50, ErrorMessage = "emailLength")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is Required.")]
        [StringLength(40, ErrorMessage = "Name Cannot Exceed 40 Characters.")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF ]+$", ErrorMessage = "Name must be letters only.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "genderReq")]
        [StringLength(10, ErrorMessage = "genderLength")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "currTypeReq")]
        [StringLength(50, ErrorMessage = "currTypeLength")]
        public string CurrentStudy { get; set; }

        [Required(ErrorMessage = "generalReq")]
        [StringLength(50, ErrorMessage = "generalLength")]
        public string GeneralMajor { get; set; }

        [Required(ErrorMessage = "specReq")]
        [StringLength(50, ErrorMessage = "specLength")]
        public string SpecificMajor { get; set; }

        [Required(ErrorMessage = "statusReq")]
        [StringLength(30, ErrorMessage = "statusLength")]
        public string Status { get; set; }

        [Required(ErrorMessage = "countryReq")]
        [StringLength(50, ErrorMessage = "countryLength")]
        public string Country { get; set; }

        [Required(ErrorMessage = "cityReq")]
        [StringLength(50, ErrorMessage = "cityLength")]
        public string City { get; set; }

        [Required(ErrorMessage = "uniReq")]
        [StringLength(60, ErrorMessage = "uniLength")]
        public string University { get; set; }

        [Required(ErrorMessage = "durationReq")]
        [StringLength(20, ErrorMessage = "durationLength")]
        public string Duration { get; set; }

        [Required]
        public DateTime Beginning_date { get; set; }

        public List<BoardModel>? Board { get; set; }

        [Required(ErrorMessage = "degreeReq")]
        [StringLength(20, ErrorMessage = "degreeLength")]
        public string Degree { get; set; }
    }
}