using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RogHotel.Models.Entity
{
    public class Prenotazione
    {
        [Key]
        public Guid PrenotazioneId { get; set; }
        [Required]
        public Guid ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        [Required]
        public Guid CameraId { get; set; }

        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }

        [Required(ErrorMessage = "Data inizio obbligatoria")]
        public DateTime DataInizio { get; set; }

        [Required(ErrorMessage = "Data fine obbligatoria")]
        public DateTime DataFine { get; set; }

        [Required(ErrorMessage = "Stato obbligatorio")]
        [StringLength(20)]
        public string Stato { get; set; }


    }
}
