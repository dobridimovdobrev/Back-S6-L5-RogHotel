using Microsoft.AspNetCore.Mvc;

namespace RogHotel.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
