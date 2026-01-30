using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
