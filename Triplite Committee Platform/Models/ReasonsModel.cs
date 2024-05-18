using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class ReasonsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReasonID { get; set; }

        [Required(ErrorMessage = "Request Type is Required.")]
        public int ReqTypeID { get; set; }

        [Required(ErrorMessage = "Context is Required.")]
        [StringLength(600, ErrorMessage = "Context Cannot Exceed 600 Characters.")]
        public string Context { get; set; }

        [Required(ErrorMessage = "Must Pick if The Reason Require Other Attribute From The User.")]
        public Boolean Connected { get; set; }

        [ForeignKey("ReqTypeID")] public RequestTypeModel? RequestType { get; set; }
    }
}