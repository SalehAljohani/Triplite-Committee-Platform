using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triplite_Committee_Platform.Models
{
    [PrimaryKey(nameof(BoardNo), nameof(UserId))]
    public class BoardSignaturesModel
    {
        public int BoardNo { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(BoardNo))]
        public BoardModel? Board { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserModel? User { get; set; }

    }
}
