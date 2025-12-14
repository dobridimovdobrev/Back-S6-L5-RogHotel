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

        private async Task LoadDropdownsAsync()
        {
            var clienti = await _clienteService.GetClientiAsync();
            var camere = await _cameraService.GetCamereAsync();

            ViewBag.Clienti = new SelectList(clienti, "ClienteId", "Nome");
            ViewBag.Camere = new SelectList(camere, "CameraId", "Numero");
            ViewBag.Stati = new SelectList(new[] { "Confermata", "Annullata", "Completata" });
        }

        public async Task<IActionResult> Index()
        {
            var prenotazioni = await _prenotazioneService.GetPrenotazioniAsync();
            return View(prenotazioni);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> CreatePartial()
        {
            await LoadDropdownsAsync();
            var prenotazione = new Prenotazione
            {
                // configuro di default giorno e ora per il check in
                DataInizio = DateTime.Today.AddHours(14),

                //voglio settare di defaul 1 giorno e orario per check out
                DataFine = DateTime.Today.AddDays(1).AddHours(10)
            };
            return PartialView("_FormPrenotazione", prenotazione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> Create(Prenotazione prenotazione)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dati non validi" });
            }

            var result = await _prenotazioneService.CreatePrenotazioneAsync(prenotazione);

            if (result)
            {
                return Json(new { success = true, message = "Prenotazione creata con successo" });
            }

            return Json(new { success = false, message = "Camera non disponibile" });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> EditPartial(Guid id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneAsync(id);

            if (prenotazione == null)
            {
                return NotFound();
            }

            await LoadDropdownsAsync();
            return PartialView("_FormPrenotazione", prenotazione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> Edit(Guid id, Prenotazione prenotazione)
        {
            if (id != prenotazione.PrenotazioneId)
            {
                return Json(new { success = false, message = "ID non corrispondente" });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dati non validi" });
            }

            var result = await _prenotazioneService.UpdatePrenotazioneAsync(prenotazione);

            if (result)
            {
                return Json(new { success = true, message = "Prenotazione aggiornata con successo" });
            }

            return Json(new { success = false, message = "Errore durante l'aggiornamento" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Dipendente")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _prenotazioneService.DeletePrenotazioneAsync(id);

            if (result)
            {
                return Json(new { success = true, message = "Prenotazione eliminata con successo" });
            }

            return Json(new { success = false, message = "Errore durante l'eliminazione" });
        }
    }
}