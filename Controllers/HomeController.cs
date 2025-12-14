using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RogHotel.Models;
using RogHotel.Services;

namespace RogHotel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PrenotazioneService _prenotazioneService;

        public HomeController(
            ILogger<HomeController> logger,
            PrenotazioneService prenotazioneService)
        {
            _logger = logger;
            _prenotazioneService = prenotazioneService;
        }

        public async Task<IActionResult> Index()
        {
            var stats = await _prenotazioneService.GetDashboardStatsAsync();
            return View(stats);
        }
    }
}