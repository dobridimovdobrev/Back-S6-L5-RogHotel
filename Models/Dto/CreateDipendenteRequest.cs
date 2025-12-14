using System.ComponentModel.DataAnnotations;

namespace RogHotel.Models.Dto
{
    public class CreateDipendenteRequest
    {
        [Required(ErrorMessage = "Nome obbligatorio")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Cognome obbligatorio")]
        [StringLength(50)]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Email obbligatoria")]
        [EmailAddress]
        [StringLength(70)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password obbligatoria")]
        [StringLength(60, MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Conferma password obbligatoria")]
        [Compare("Password", ErrorMessage = "Le password non coincidono")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Ruolo obbligatorio")]
        public string Ruolo { get; set; }
    }
}