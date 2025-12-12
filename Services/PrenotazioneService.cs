using Microsoft.EntityFrameworkCore;
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