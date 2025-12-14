using RogHotel.Models.Entity;

namespace RogHotel.Models
{
    public class DashboardViewModel
    {
        public int ArriviOggi { get; set; }
        public int PartenzeOggi { get; set; }
        public int CamereDisponibili { get; set; }
        public int CamereTotali { get; set; }
        public int TotaleClienti { get; set; }

        // prossimi arrivi lista
        public List<Prenotazione> ProssimiArrivi { get; set; }
    }
}