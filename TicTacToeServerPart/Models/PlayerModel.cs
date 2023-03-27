using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToeServerPart.Models
{
    public class PlayerModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = "";

        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; } = "";

        public DateTime RegistrationDate { get; private set; } = DateTime.Now;

        public int Scores { get; set; } = default;

        public int LoginModelId { get; set; }

        public LoginModel LoginModel { get; set; } = null!;
    }
}
