using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class BoardModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoardNo { get; set; }

        [Required(ErrorMessage = "boardTitle")]
        [StringLength(200, ErrorMessage = "boardLength")]
        public string Title { get; set; }

        [Required(ErrorMessage = "reasonsReq")]
        public string Reasons { get; set; }
        public string? AddedReasons { get; set; }
        public bool? Recommendation { get; set; }
        public bool? HeadofDeptSign { get; set; }
        public bool? ViceDeanSign { get; set; }
        public bool? DeanSign { get; set; }
        public bool? DeptMemeberSign1 { get; set; }
        public bool? DeptMemeberSign2 { get; set; }

        [ForeignKey("DeptNo")]
        public DepartmentModel? Department { get; set; }

        [Required(ErrorMessage = "pickDept")]
        public int DeptNo { get; set; }

        public List<FileModel>? File { get; set; }

        [Required(ErrorMessage = "idReq")]
        public int Id { get; set; }

        [Required(ErrorMessage = "dateReq")]
        public DateTime ReqDate { get; set; }

        [Required(ErrorMessage = "statusReq")]
        [StringLength(20, ErrorMessage = "statusLength")]
        public string ReqStatus { get; set; }

        [Required(ErrorMessage = "typeReq")]
        public int ReqTypeID { get; set; }

        [ForeignKey("ReqTypeID")] public RequestTypeModel? RequestType { get; set; }

        [ForeignKey("Id")] public ScholarshipModel? Scholarship { get; set; }

        public ICollection<BoardSignaturesModel>? BoardSignatures { get; set; }
    }
}