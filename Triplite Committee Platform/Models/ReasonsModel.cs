using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Triplite_Committee_Platform.Models
{
    public class ReasonsModel
    {
        [Key] public int ReasonID { get; set; }
        [Required] public int ReqTypeID  { get; set; }
        [Required] public string Context { get; set; }
        [Required] public Boolean Connected { get; set; }

        [ForeignKey("ReqTypeID")] public RequestTypeModel RequestType { get; set; }

    }
}
