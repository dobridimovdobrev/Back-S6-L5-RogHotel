using System.ComponentModel.DataAnnotations;

namespace RogHotel.Models.Entity
{
    public class Cliente
    {
        [Key]
        public Guid ClienteId { get; set; }
        [Required(ErrorMessage = "Nome obbligatorio")]
        [StringLength(50)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Cognome obbligatorio")]
        [StringLength(50)]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Email obbligatorio")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefono obbligatorio")]
        [StringLength(20)]
        public string Telefono { get; set; }

        // relazione molti a molti
        public ICollection<Prenotazione>? Prenotazioni { get; set; }

    }
}
