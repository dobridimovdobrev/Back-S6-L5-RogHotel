using System.ComponentModel.DataAnnotations;

namespace RogHotel.Models.Dto
{
    public class EditDipendenteRequest
    {
        [Required]
        public string IdDipendente { get; set; }

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

        [Required(ErrorMessage = "Ruolo obbligatorio")]
        public string Ruolo { get; set; }
    }
}
