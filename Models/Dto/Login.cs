using System.ComponentModel.DataAnnotations;

namespace RogHotel.Models.Dto
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email obbligatoria")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password obbligatoria")]
        public string Password { get; set; }
    }
}