using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Triplite_Committee_Platform.Models
{

    public class RequestTypeModel
    {
        [Key] public int RequestTypeID { get; set; }
        [Required] public string RequestTypeName { get; set; }
        public List<BoardModel> Board { get; set; } 
        public List<ReasonsModel> Reasons { get; set; }
    }
}
//ddddd