using System.ComponentModel.DataAnnotations;

namespace Triplite_Committee_Platform.Models
{
    public class RolesModels
    {
        [Key] public int RoleID { get; set; }
        [Required] public string RoleName { get; set; }
    }
}
