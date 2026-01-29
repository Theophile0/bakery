using System.Diagnostics;
using Bakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bakery.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error")]
        public IActionResult Index()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            return View(new ErrorViewModel
            {
                RequestId = requestId
            });
        }
    }
}
