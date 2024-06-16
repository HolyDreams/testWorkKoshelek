using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebSocketOptions = Core.Domain.Models.Settings.WebSocketOptions;

namespace testWorkKoshelek.ReaderClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebSocketOptions _options;

        public HomeController(IOptions<WebSocketOptions> options)
        {
            _options = options.Value;
        }

        public IActionResult Index()
        {
            TempData["Uri"] = _options.Uri;
            return View();
        }
    }
}