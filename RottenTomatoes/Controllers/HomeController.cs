using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RottenTomatoes.Data;
using RottenTomatoes.Models;
using System.Diagnostics;
using System.Security.Policy;
using System.Text;

namespace RottenTomatoes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RottenTomatoesContext _context;

        public HomeController(ILogger<HomeController> logger, RottenTomatoesContext context)
        {
            _logger = logger;
            _context = context;
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

        public async Task<IActionResult> CreateMovie([Bind("MovieId,CriticReview,AudienceReview,Clasification,OriginalLanguage,Director,Runtime,ActorRoles,Title,ImageURL,TomatometerScore,AudienceScore,Platforms,Synopsis,Genre,ReleaseDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CreateSerie([Bind("SerieId,Title,ImageURL,TomatometerScore,AudienceScore,Platforms,Synopsis,Genre,ReleaseDate")] Serie serie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serie);
        }
        public async Task<IActionResult> AgregarShow()
        {
            var showType = Request.Form["showType"];
            var url = Request.Form["url"];

            var scraper = new WebScraper(showType, url);



            Show show = await scraper.GetShowInfo();

            if (show is Movie)
            {
                Movie movie = (Movie)show;
                await CreateMovie(movie);

            }
            else if (show is Serie)
            {
                Serie serie = (Serie)show;
                await CreateSerie(serie);
            }
            else
            {
                // El objeto no es de ninguna de las dos clases
            }


            // Aquí puedes hacer lo que necesites con los datos ingresados por el usuario, como agregarlos a una base de datos o enviarlos a otro servicio
            TempData["Success"] = true;
            return RedirectToAction("RTScrapper"); // Redirige al usuario a la vista RTscrapper después de enviar los datos
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}