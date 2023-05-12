using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RottenTomatoes.Data;
using RottenTomatoes.Models;
using System.Data.Entity.Core.Metadata.Edm;
using System;
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
        public IActionResult SmartSearch()
        {

            return View();
        }
        public IActionResult BingChat(string showType, string prompt)
        {
            string respuesta = "En desarrollo...";

            // Pasar la respuesta a la vista SmartSearch
            ViewBag.Respuesta = respuesta;

            // Redirigir a la vista SmartSearch
            return View("SmartSearch");

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
            var userType = Request.Cookies["UserType"];

            if (userType == "Experto en cine")
            {
                // Mostrar vista solo para usuarios expertos en cine
                return View();
            }
            else
            {
                // Redirigir a la página de inicio si el usuario no es un experto en cine
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Recomendaciones()
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
        public IActionResult DeleteAllMovies()
        {
            try
            {
                _context.Movie.RemoveRange(_context.Movie);
                _context.SaveChanges();
                return Ok("Todos los datos de la tabla Movies han sido eliminados.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al eliminar los datos de la tabla Movies: " + ex.Message);
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}