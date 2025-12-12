using Microsoft.AspNetCore.Mvc;

namespace RogHotel.Controllers
{
    public class CameraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
