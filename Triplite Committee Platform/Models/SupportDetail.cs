using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class SupportDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="emailReq")]
        [StringLength(35, ErrorMessage ="emailLength")]
        public string ContactEmail { get; set; }

        [StringLength(10, ErrorMessage = "phoneLength")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "phoneValid")]
        public string? PhoneNumber { get; set; }

        [StringLength(15, ErrorMessage = "teleLength")]
        [RegularExpression(@"^([0-9]{15})$", ErrorMessage = "teleValid")]
        public string? TeleNumber { get; set; }

        [Required(ErrorMessage = "locReq")]
        public string Location { get; set; }
    }
}
