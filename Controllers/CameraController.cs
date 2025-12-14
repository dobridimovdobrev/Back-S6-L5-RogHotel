using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RogHotel.Models.Entity;
using RogHotel.Services;

namespace RogHotel.Controllers
{
    [Authorize(Roles = "Admin,Dipendente")]
    public class CameraController : Controller
    {
        private readonly CameraService _cameraService;

        public CameraController(CameraService cameraService)
        {
            _cameraService = cameraService;
        }

        private void LoadDropdowns()
        {
            ViewBag.TipiCamera = new SelectList(new[]
            {
                "Singola",
                "Doppia",
                "Suite",
                "Deluxe",
                "Uncle Rog Premium"
            });
        }

        // lista camere
        public async Task<IActionResult> Index()
        {
            var camere = await _cameraService.GetCamereAsync();
            return View(camere);
        }

        // metodo Get form 
        [HttpGet]
        public IActionResult CreatePartial()
        {
            LoadDropdowns();
            var camera = new Camera();
            return PartialView("_FormCamera", camera);
        }

        // salva camera form ajax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Camera camera)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dati non validi" });
            }

            var result = await _cameraService.CreateCameraAsync(camera);

            if (result)
            {
                return Json(new { success = true, message = "Camera creata con successo" });
            }

            return Json(new { success = false, message = "Errore durante la creazione" });
        }

        // metodo Get modifica form
        [HttpGet]
        public async Task<IActionResult> EditPartial(Guid id)
        {
            var camera = await _cameraService.GetCameraAsync(id);

            if (camera == null)
            {
                return NotFound();
            }

            LoadDropdowns();
            return PartialView("_FormCamera", camera);
        }

        // modofica camera ajax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Camera camera)
        {
            if (id != camera.CameraId)
            {
                return Json(new { success = false, message = "ID non corrispondente" });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dati non validi" });
            }

            var result = await _cameraService.UpdateCameraAsync(camera);

            if (result)
            {
                return Json(new { success = true, message = "Camera aggiornata con successo" });
            }

            return Json(new { success = false, message = "Errore durante l'aggiornamento" });
        }

        // elimina camera ajax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _cameraService.DeleteCameraAsync(id);

            if (result)
            {
                return Json(new { success = true, message = "Camera eliminata con successo" });
            }

            return Json(new { success = false, message = "Errore durante l'eliminazione" });
        }
    }
}