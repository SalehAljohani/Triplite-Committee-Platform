using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class ContactModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "nameReq")]
        [StringLength(35, ErrorMessage = "nameLength")]
        [RegularExpression(@"^[a-zA-Z\s\u0600-\u06FF]*$", ErrorMessage = "nameCond")]
        public string Name { get; set; }

        [Required(ErrorMessage = "emailReq")]
        [EmailAddress(ErrorMessage = "emailValid")]
        [StringLength(30, ErrorMessage = "emailLength")]
        public string Email { get; set; }

        [Required(ErrorMessage = "subjReq")]
        [StringLength(40, ErrorMessage = "subjLength")]
        [RegularExpression(@"^[a-zA-Z\s\u0600-\u06FF]*$", ErrorMessage = "subjCond")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "msgReq")]
        [StringLength(500, ErrorMessage = "msgLength")]
        public string Message { get; set; }

        public bool Status { get; set; }

    }
}
