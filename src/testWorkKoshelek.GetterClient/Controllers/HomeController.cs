using Core.Domain.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using testWorkKoshelek.GetterClient.Models;

namespace testWorkKoshelek.GetterClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiOptions _options;

        public HomeController(IOptions<ApiOptions> options)
        {
            _options = options.Value;
        }

        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.Uri);

                var parameters = new Dictionary<string, string>
                {
                    { "start", DateTime.Now.AddMinutes(-100).ToString("yyyy.MM.dd HH:mm:ss") }
                };

                var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeUriString(p.Value)}"));

                var response = await client.GetAsync("message/getByPeriod?" + queryString);

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<MessageModel>>(responseString);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = result;
                    return View(result);
                }
            }

            return View(null);
        }
    }
}
