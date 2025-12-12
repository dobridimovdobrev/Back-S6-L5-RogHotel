using Microsoft.AspNetCore.Mvc;

namespace RogHotel.Controllers
{
    public class ApplicationUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
