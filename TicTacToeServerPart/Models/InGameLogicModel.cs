using System.ComponentModel.DataAnnotations;

namespace TicTacToeServerPart.Models
{
    public class InGameLogicModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int FirstPlayerId { get; set; } = 0;

        [Required]
        public int SecondPlayerId { get; set; } = 0;

        public int WinnerId { get; set; } = 0;

        [Required]
        public bool OnLine { get; set; } = true;

        [Required]
        public bool PublicSearchType { get; set; } = true;
    }
}
