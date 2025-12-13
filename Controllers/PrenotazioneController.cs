using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RogHotel.Models.Entity;
using RogHotel.Services;

namespace RogHotel.Controllers
{
    [Authorize]
    public class PrenotazioneController : Controller
    {
        private readonly PrenotazioneService _prenotazioneService;
        private readonly ClienteService _clienteService;
        private readonly CameraService _cameraService;

        public PrenotazioneController(
            PrenotazioneService prenotazioneService,
            ClienteService clienteService,
            CameraService cameraService)
        {
            _prenotazioneService = prenotazioneService;
            _clienteService = clienteService;
            _cameraService = cameraService;
        }

        public async Task<IActionResult> Index()
        {
            var prenotazioni = await _prenotazioneService.GetPrenotazioniAsync();
            return View(prenotazioni);
        }

        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> Create()
        {

            var clienti = await _clienteService.GetClientiAsync();
            var camere = await _cameraService.GetCamereAsync();

            ViewBag.Clienti = new SelectList(clienti, "ClienteId", "Nome");
            ViewBag.Camere = new SelectList(camere, "CameraId", "Numero");
            ViewBag.Stati = new SelectList(new[] { "Confermata", "Annullata", "Completata" });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> Create(Prenotazione prenotazione)
        {
            if (!ModelState.IsValid)
            {

                var clienti = await _clienteService.GetClientiAsync();
                var camere = await _cameraService.GetCamereAsync();

                ViewBag.Clienti = new SelectList(clienti, "ClienteId", "Nome");
                ViewBag.Camere = new SelectList(camere, "CameraId", "Numero");
                ViewBag.Stati = new SelectList(new[] { "Confermata", "Annullata", "Completata" });

                return View(prenotazione);
            }

            var result = await _prenotazioneService.CreatePrenotazioneAsync(prenotazione);

            if (result)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Camera non disponibile");


            var clienti2 = await _clienteService.GetClientiAsync();
            var camere2 = await _cameraService.GetCamereAsync();

            ViewBag.Clienti = new SelectList(clienti2, "ClienteId", "Nome");
            ViewBag.Camere = new SelectList(camere2, "CameraId", "Numero");
            ViewBag.Stati = new SelectList(new[] { "Confermata", "Annullata", "Completata" });

            return View(prenotazione);
        }

        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneAsync(id);
            if (prenotazione == null)
                return NotFound();


            var clienti = await _clienteService.GetClientiAsync();
            var camere = await _cameraService.GetCamereAsync();

            ViewBag.Clienti = new SelectList(clienti, "ClienteId", "Nome");
            ViewBag.Camere = new SelectList(camere, "CameraId", "Numero");
            ViewBag.Stati = new SelectList(new[] { "Confermata", "Annullata", "Completata" });

            return View(prenotazione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> Edit(Guid id, Prenotazione prenotazione)
        {
            if (id != prenotazione.PrenotazioneId)
                return NotFound();

            if (!ModelState.IsValid)
            {

                var clienti = await _clienteService.GetClientiAsync();
                var camere = await _cameraService.GetCamereAsync();

                ViewBag.Clienti = new SelectList(clienti, "ClienteId", "Nome");
                ViewBag.Camere = new SelectList(camere, "CameraId", "Numero");
                ViewBag.Stati = new SelectList(new[] { "Confermata", "Annullata", "Completata" });

                return View(prenotazione);
            }

            var result = await _prenotazioneService.UpdatePrenotazioneAsync(prenotazione);

            if (result)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Errore");


            var clienti2 = await _clienteService.GetClientiAsync();
            var camere2 = await _cameraService.GetCamereAsync();

            ViewBag.Clienti = new SelectList(clienti2, "ClienteId", "Nome");
            ViewBag.Camere = new SelectList(camere2, "CameraId", "Numero");
            ViewBag.Stati = new SelectList(new[] { "Confermata", "Annullata", "Completata" });

            return View(prenotazione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _prenotazioneService.DeletePrenotazioneAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}