using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RogHotel.Models.Entity
{
    public class Camera
    {
        [Key]
        public Guid CameraId { get; set; }

        [Required(ErrorMessage = "Numero camera obbligatorio")]
        [StringLength(10)]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Tipo camera obbligatorio")]
        [StringLength(20)]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Prezzo obbligatorio")]
        [Range(0.01, 20000)]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Prezzo { get; set; }

        //relazioni molti a motli
        public ICollection<Prenotazione>? Prenotazioni { get; set; }

    }
}
