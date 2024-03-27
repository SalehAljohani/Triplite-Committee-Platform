using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class BoardModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoardNo { get; set; }

        [ForeignKey("DeptNo")] public DepartmentModel? Department { get; set; }

        [Required (ErrorMessage = "Must Pick a Department.")]
        public int DeptNo { get; set; }

        public List<FileModel>? File { get; set; }

        [Required (ErrorMessage = "Student National ID is Required.")]
        public int National_ID { get; set; }

        [Required(ErrorMessage = "Request Date is Required.")]
        public DateTime ReqDate { get; set; }

        [Required(ErrorMessage = "Request Status is Required.")]
        [StringLength(20, ErrorMessage = "Request Status Cannot Exceed 20 characters.")]
        public string ReqStatus { get; set; }

        [Required(ErrorMessage = "Request Type is Required.")]
        public int ReqTypeID { get; set; }

        [ForeignKey("ReqTypeID")] public RequestTypeModel? RequestType { get; set; }

        [ForeignKey("National_ID")] public ScholarshipModel? Scholarship { get; set; }
    }
}