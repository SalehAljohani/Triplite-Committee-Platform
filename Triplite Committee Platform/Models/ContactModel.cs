using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class ContactModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Your Name is Required.")]
        [StringLength(35, ErrorMessage = "Name Cannot Exceed 35 Characters.")]
        [RegularExpression(@"^[a-zA-Z\s\u0600-\u06FF]*$", ErrorMessage = "Name can only contain letters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(30, ErrorMessage = "Email Cannot Exceed 30 Characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is Required.")]
        [StringLength(40, ErrorMessage = "Subject Cannot Exceed 40 Characters.")]
        [RegularExpression(@"^[a-zA-Z\s\u0600-\u06FF]*$", ErrorMessage = "Subject can only contain letters.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is Required.")]
        [StringLength(500, ErrorMessage = "Message Cannot Exceed 500 Characters.")]
        public string Message { get; set; }

        public bool Status { get; set; }

    }
}
