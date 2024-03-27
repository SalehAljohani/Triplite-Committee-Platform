using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Triplite_Committee_Platform.Models
{

    public class RequestTypeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestTypeID { get; set; }
        [Required(ErrorMessage = "Request Type Name is Required.")]
        [StringLength(30, ErrorMessage = "Request Type Name Cannot Exceed 30 Characters.")]
        public string RequestTypeName { get; set; }
        public List<BoardModel>? Board { get; set; }
        public List<ReasonsModel>? Reasons { get; set; }
    }
}