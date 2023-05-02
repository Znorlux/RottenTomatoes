using Microsoft.AspNetCore.Mvc;
using RottenTomatoes.Models;
using System.Diagnostics;

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
            // lógica para mostrar la información obtenida


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