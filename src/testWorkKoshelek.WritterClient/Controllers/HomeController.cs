using Core.Domain.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using testWorkKoshelek.WritterClient.Models;

namespace testWorkKoshelek.WritterClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiOptions _options;

        public HomeController(IOptions<ApiOptions> options)
        {
            _options = options.Value;
            Console.WriteLine(_options.Uri);
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(SendModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.Uri);

                var postMessage = client.PostAsJsonAsync("Message/send", model);
                postMessage.Wait();

                var result = postMessage.Result;

                if (result.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Сообщение успешно отправленно!";
                    return RedirectToAction("Create");
                }
            }

            ViewData["Error"] = "Не удалось отправить сообщение.";
            return View(model);
        }
    }
}
