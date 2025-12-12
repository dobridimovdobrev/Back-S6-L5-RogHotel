using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RogHotel.Models.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Nome obbligatorio")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Cognome obbligatorio")]
        [StringLength(50)]
        public string Cognome { get; set; }

        public DateTime DataCreazione { get; set; }

    }
}
