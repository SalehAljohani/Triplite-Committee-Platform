using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class SupportPageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Contact Email is Required")]
        [StringLength(35, ErrorMessage ="Contact Email cannot exceed 35 characters")]
        public string ContactEmail { get; set; }

        [StringLength(10, ErrorMessage = "Phone Number cannot exceed 10 characters")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(15, ErrorMessage = "Telephone Number cannot exceed 15 characters")]
        [RegularExpression(@"^([0-9]{15})$", ErrorMessage = "Invalid Telephone Number")]
        public string? TeleNumber { get; set; }

        [Required(ErrorMessage = "Location is Required")]
        public string Location { get; set; }
    }
}
