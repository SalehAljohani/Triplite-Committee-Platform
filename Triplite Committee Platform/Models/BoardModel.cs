using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Triplite_Committee_Platform.Models
{
    public class BoardModel // Board Model when migrated create two rows of National_ID for some reason
    {
        [Key] public int BoardNo { get; set; }
        [Required] public int ReqTypeID { get; set; }
        [ForeignKey("ReqTypeID")] public RequestTypeModel RequestType { get; set; }
        [Required] public int National_ID { get; set; }
        [ForeignKey("National_ID")] public ScholarshipModel Scholarship { get; set; }
        [Required] public int DeptNo { get; set; }
        [ForeignKey("DeptNo")] public DepartmentModel Department { get; set; }
        [Required] public string ReqStatus { get; set; }
        [Required] public DateTime ReqDate { get; set; }
        public List<FileModel> File { get; set; }
    }
}