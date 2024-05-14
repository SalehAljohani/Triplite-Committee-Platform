using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class FileModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileID { get; set; }

        [Required(ErrorMessage = "nameReq")]
        [StringLength(25)]
        public string FileName { get; set; }

        public int BoardNo { get; set; }
        [ForeignKey("BoardNo")] public BoardModel? Board { get; set; }

        [Required(ErrorMessage = "regDate")]
        public string Creation_Date { get; set; }
    }
}