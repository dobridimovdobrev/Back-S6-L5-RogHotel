using Microsoft.EntityFrameworkCore;
using RogHotel.Models;
using RogHotel.Models.Entity;

namespace RogHotel.Services
{
    public class PrenotazioneService : ServiceBase
    {
        public PrenotazioneService(ApplicationDbContext context) : base(context)
        {
        }

        // lista prenotazioni
        public async Task<List<Prenotazione>> GetPrenotazioniAsync()
        {
            return await _context.Prenotazioni
                .AsNoTracking()
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .ToListAsync();
        }

        // prenotazione by id
        public async Task<Prenotazione?> GetPrenotazioneAsync(Guid id)
        {
            return await _context.Prenotazioni
                .AsNoTracking()
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .FirstOrDefaultAsync(p => p.PrenotazioneId == id);
        }

        //creare prenotazione
        public async Task<bool> CreatePrenotazioneAsync(Prenotazione prenotazione)
        {
            try
            {

                bool disponibile = await DisponibilitaPrenotazioniAsync(
                    prenotazione.CameraId,
                    prenotazione.DataInizio,
                    prenotazione.DataFine);

                if (!disponibile)
                    return false;

                prenotazione.PrenotazioneId = Guid.NewGuid();
                _context.Prenotazioni.Add(prenotazione);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        // aggiorna prenotazione
        public async Task<bool> UpdatePrenotazioneAsync(Prenotazione prenotazione)
        {
            try
            {
                _context.Prenotazioni.Update(prenotazione);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //elimina prenotazione
        public async Task<bool> DeletePrenotazioneAsync(Guid id)
        {
            try
            {
                var prenotazione = await _context.Prenotazioni.FindAsync(id);

                if (prenotazione is null)
                    return false;

                _context.Prenotazioni.Remove(prenotazione);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        // statistiche
        public async Task<DashboardViewModel> GetDashboardStatsAsync()
        {
            var oggi = DateTime.Today;
            var domani = oggi.AddDays(1);

            var stats = new DashboardViewModel
            {
                // arrivi 
                ArriviOggi = await _context.Prenotazioni
                    .Where(p => p.DataInizio.Date == oggi && p.Stato == "Confermata")
                    .CountAsync(),

                // partenze oggi 
                PartenzeOggi = await _context.Prenotazioni
                    .Where(p => p.DataFine.Date == oggi && p.Stato == "Confermata")
                    .CountAsync(),

                // camere totali
                CamereTotali = await _context.Camere.CountAsync(),

                // camere occupate
                CamereDisponibili = await _context.Camere.CountAsync() -
                    await _context.Prenotazioni
                        .Where(p => oggi >= p.DataInizio.Date &&
                                   oggi < p.DataFine.Date &&
                                   p.Stato == "Confermata")
                        .Select(p => p.CameraId)
                        .Distinct()
                        .CountAsync(),

                // totale clienti
                TotaleClienti = await _context.Clienti.CountAsync(),

                // prossimi 6 arrivi 
                ProssimiArrivi = await _context.Prenotazioni
                    .Include(p => p.Cliente)
                    .Include(p => p.Camera)
                    .Where(p => p.DataInizio.Date >= oggi && p.Stato == "Confermata")
                    .OrderBy(p => p.DataInizio)
                    .Take(6)
                    .ToListAsync()
            };

            return stats;
        }

        // disonibiulita prenotazioni
        private async Task<bool> DisponibilitaPrenotazioniAsync(Guid cameraId, DateTime dataInizio, DateTime dataFine)
        {
            var cameraOccupata = await _context.Prenotazioni
                .Where(p => p.CameraId == cameraId &&
                           p.Stato != "Annullata" &&
                           ((dataInizio >= p.DataInizio && dataInizio < p.DataFine) ||
                            (dataFine > p.DataInizio && dataFine <= p.DataFine) ||
                            (dataInizio <= p.DataInizio && dataFine >= p.DataFine)))
                .ToListAsync();

            return cameraOccupata.Count == 0;
        }
    }
}