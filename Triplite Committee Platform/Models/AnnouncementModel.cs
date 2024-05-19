using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class AnnouncementModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "titleReq1")]
        [StringLength(100, ErrorMessage = "titleReq2")]
        public string Title { get; set; }

        [Required(ErrorMessage = "descReq1")]
        [StringLength(600, ErrorMessage = "descReq2")]
        public string Description { get; set; }

        [Display(Name = "fileLink")]
        public string? Link { get; set; }

        [ForeignKey("DeptNo")]
        public DepartmentModel? Department { get; set; }

        [Required(ErrorMessage = "pickDept")]
        public int DeptNo { get; set; }
    }
}
