using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Triplite_Committee_Platform.Models
{

    public class RequestTypeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestTypeID { get; set; }
        [Required(ErrorMessage = "reqType")]
        [StringLength(30, ErrorMessage = "reqTypeLength")]
        public string RequestTypeName { get; set; }
        public List<BoardModel>? Board { get; set; }
        public ReasonsModel? Reasons { get; set; }
    }
}