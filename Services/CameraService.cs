using Microsoft.EntityFrameworkCore;
using RogHotel.Models.Entity;

namespace RogHotel.Services
{
    public class CameraService : ServiceBase
    {
        public CameraService(ApplicationDbContext context) : base(context)
        {
        }

        // lista camere
        public async Task<List<Camera>> GetCamereAsync()
        {
            try
            {
                return await _context.Camere.AsNoTracking().ToListAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Camera>();
            }
        }

        // camera by id
        public async Task<Camera?> GetCameraAsync(Guid id)
        {
            try
            {
                return await _context.Camere.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // registrare camera
        public async Task<bool> CreateCameraAsync(Camera camera)
        {
            try
            {
                _context.Camere.Add(camera);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //aggiornare camera
        public async Task<bool> UpdateCameraAsync(Camera camera)
        {
            try
            {
                _context.Camere.Update(camera);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //eliminare camera
        public async Task<bool> DeleteCameraAsync(Guid id)
        {
            try
            {
                var camera = await _context.Camere.FindAsync(id);
                if (camera == null) return false;

                _context.Camere.Remove(camera);
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