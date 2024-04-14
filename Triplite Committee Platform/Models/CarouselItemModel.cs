using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    public class CarouselItemModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required.")]
        [StringLength(100, ErrorMessage = "Title Cannot Exceed 100 Characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is Required.")]
        [StringLength(600, ErrorMessage = "Description Cannot Exceed 600 Characters.")]
        public string Description { get; set; }

        [Display(Name = "File Link")]
        public string? Link { get; set; }
    }
}
