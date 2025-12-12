using Microsoft.EntityFrameworkCore;
using RogHotel.Models.Entity;

namespace RogHotel.Services
{
    public class ClienteService : ServiceBase
    {
        public ClienteService(ApplicationDbContext context) : base(context)
        {
        }
        // lista clienti
        public async Task<List<Cliente>> GetClientiAsync()
        {
            try
            {
                return await _context.Clienti
                    .AsNoTracking()
                    .OrderBy(a => a.Cognome)
                    .ThenBy(a => a.Nome)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Cliente>();
            }
        }
        // cliente by id
        public async Task<Cliente?> GetClienteAsync(Guid id)
        {
            try
            {
                return await _context.Clienti
                    .AsNoTracking().
                    FirstOrDefaultAsync(a => a.ClienteId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        // registrare cliente
        public async Task<bool> CreateClienteAsync(Cliente cliente)
        {
            try
            {
                await _context.Clienti.AddAsync(cliente);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // aggiornare cliente
        public async Task<bool> UpdateClienteAsync(Cliente cliente)
        {
            try
            {
                _context.Clienti.Update(cliente);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //elimina cliente
        public async Task<bool> DeleteClienteAsync(Guid id)
        {
            try
            {
                var cliente = await _context.Clienti.FindAsync(id);

                if (cliente is null)
                    return false;

                _context.Clienti.Remove(cliente);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}