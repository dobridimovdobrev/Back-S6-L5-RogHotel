using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RogHotel.Models.Entity
{
    // id sono stringe, mi ci vuole tempo per abituarmi :)
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // non mi e chiaro anche se non succede nulla perche usarli, tanto identtity ce li ha queste

        //public DbSet<ApplicationUser> AspNetUsers { get; set; }
        //public DbSet<ApplicationRole> AspNetRoles { get; set; }
        public DbSet<Cliente> Clienti { get; set; }
        public DbSet<Camera> Camere { get; set; }
        public DbSet<Prenotazione> Prenotazioni { get; set; }

    }
}
