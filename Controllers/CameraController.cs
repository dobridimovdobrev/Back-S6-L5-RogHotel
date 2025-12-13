using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            var camere = await _cameraService.GetCamereAsync();
            return View(camere);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Camera camera)
        {
            if (!ModelState.IsValid)
                return View(camera);

            var result = await _cameraService.CreateCameraAsync(camera);

            if (result)
                return RedirectToAction(nameof(Index));

            return View(camera);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var camera = await _cameraService.GetCameraAsync(id);
            if (camera == null)
                return NotFound();

            return View(camera);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Camera camera)
        {
            if (id != camera.CameraId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(camera);

            var result = await _cameraService.UpdateCameraAsync(camera);

            if (result)
                return RedirectToAction(nameof(Index));

            return View(camera);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cameraService.DeleteCameraAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}