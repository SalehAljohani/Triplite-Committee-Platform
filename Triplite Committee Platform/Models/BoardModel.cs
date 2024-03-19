using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class BoardModel
    {
        [Key]
        public int BoardNo { get; set; }

        [ForeignKey("DeptNo")] public DepartmentModel Department { get; set; }

        [Required]
        public int DeptNo { get; set; }

        public List<FileModel> File { get; set; }

        [Required]
        public int National_ID { get; set; }

        [Required]
        public DateTime ReqDate { get; set; }

        [Required]
        [StringLength(20)]
        public string ReqStatus { get; set; }

        [Required]
        public int ReqTypeID { get; set; }

        [ForeignKey("ReqTypeID")] public RequestTypeModel RequestType { get; set; }

        [ForeignKey("National_ID")] public ScholarshipModel Scholarship { get; set; }
    }
}