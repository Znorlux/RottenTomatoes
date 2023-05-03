using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RottenTomatoes.Models;
using System.Diagnostics;
using System.Text;

namespace RottenTomatoes.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
        public IActionResult RTscrapper()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AgregarShow()

        {
            var showType = Request.Form["showType"];
            var url = Request.Form["url"];

            var scraper = new WebScraper(showType, url);
            Show show = await scraper.GetShowInfo();

            if (show is Movie movie)
            {
                HttpClient client = new HttpClient();

                // Serialize the movie object to JSON
                string json = JsonConvert.SerializeObject(movie);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the HTTP POST request to the MoviesController's Create method
                HttpResponseMessage response = await client.PostAsync("https://localhost:7159/Movies/Create", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("Error");
                }
            }
            else if (show is Serie)
            {
                // El objeto es de la clase Serie
            }
            else
            {
                // El objeto no es de ninguna de las dos clases
            }


            // Aquí puedes hacer lo que necesites con los datos ingresados por el usuario, como agregarlos a una base de datos o enviarlos a otro servicio
            TempData["Success"] = true;
            return RedirectToAction("RTscrapper"); // Redirige al usuario a la vista RTscrapper después de enviar los datos
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}