using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TicTacToeServerPart.Models
{
    public class LoginModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string EmailAddress { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}
