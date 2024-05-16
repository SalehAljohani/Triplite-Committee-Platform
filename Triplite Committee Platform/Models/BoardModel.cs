using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class BoardModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoardNo { get; set; }

        [Required(ErrorMessage = "Board Title is Required.")]
        [StringLength(200, ErrorMessage = "Board Title Cannot Exceed 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Board Reasons is Required.")]
        public string Reasons { get; set; }
        public string? AddedReasons { get; set; }
        public string? Recommendation { get; set; }
        public bool Decision { get; set; }
        public bool HeadofDeptSign { get; set; }
        public bool ViceDeanSign { get; set; }
        public bool DeanSign { get; set; }
        public bool DeptMemeberSign1 { get; set; }
        public bool DeptMemeberSign2 { get; set; }

        [ForeignKey("DeptNo")]
        public DepartmentModel? Department { get; set; }

        [Required(ErrorMessage = "Must Pick a Department.")]
        public int DeptNo { get; set; }

        public List<FileModel>? File { get; set; }

        [Required(ErrorMessage = "Student National ID is Required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Request Date is Required.")]
        public DateTime ReqDate { get; set; }

        [Required(ErrorMessage = "Request Status is Required.")]
        [StringLength(20, ErrorMessage = "Request Status Cannot Exceed 20 characters.")]
        public string ReqStatus { get; set; }

        [Required(ErrorMessage = "Request Type is Required.")]
        public int ReqTypeID { get; set; }

        [ForeignKey("ReqTypeID")] public RequestTypeModel? RequestType { get; set; }

        [ForeignKey("Id")] public ScholarshipModel? Scholarship { get; set; }

        public List<string>? BoardSignatures { get; set; }
        public List<string>? UserSignatures { get; set; }
    }
}