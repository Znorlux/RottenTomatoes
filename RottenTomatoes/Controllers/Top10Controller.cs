using RottenTomatoes.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RottenTomatoes.Data;
using RottenTomatoes.Models;

namespace RottenTomatoes.Controllers
{
    public class Top10Controller : Controller
    {
        private readonly RottenTomatoesContext _context;

        public Top10Controller(RottenTomatoesContext context)
        {
            _context = context;
        }

        // GET: Top10
        public async Task add_top10()
        {
            var scraper = new WebScraper("top10", null);
            List<List<Top10>> top10 = await scraper.getTop10();
            int movieCount = 0;
            int serieCount = 0; 
            foreach (var top10value in top10)
            {
                foreach (var top in top10value)
                {
                    await Create(top);//Con el primer elemento del top, lo mandamos a la base de datos de Top10

                    if (movieCount < 10)
                    {
                        var currentMovie = new WebScraper("pelicula", top.url);
                        Show show = await currentMovie.GetShowInfo();
                        Movie movie = (Movie)show;

                        // Verificamos si ya existe una película con el mismo título y fecha de lanzamiento
                        var existingMovie = await _context.Movie.FirstOrDefaultAsync(m => m.Title == movie.Title);

                        if (existingMovie == null)
                        {
                            await _context.Movie.AddAsync(movie);
                            await _context.SaveChangesAsync();
                            
                        }
                        movieCount++;
                    }
                    //series
                    else if(serieCount < 10)
                    {
                        var currentSerie = new WebScraper("serie", top.url);
                        Show show = await currentSerie.GetShowInfo();
                        Serie serie = (Serie)show;

                        var existingSerie = await _context.Serie.FirstOrDefaultAsync(s => s.Title == serie.Title);
                        if(existingSerie == null)
                        {
                            await _context.Serie.AddAsync(serie); 
                            await _context.SaveChangesAsync();
                        }
                        serieCount++;
                    }
                }//Al finalizar este metodo se habran creado objetos de tipo top10 y añadidos a la base de datos
            }//Screapeado todos los elementos
        }//Creado objetos de tipo Movie y Serie y enviados a su BD correspondiente


        public async Task<IActionResult> Loading()
        {
            // Mostrar la vista de carga
            return View();
        }

        public async Task<IActionResult> Index()
        {
            // Ejecutar ambas tareas en paralelo

            
            // Redirigir al usuario a la vista de Top10
            return _context.Top10 != null ?
                View(await _context.Top10.ToListAsync()) :
                Problem("Entity set 'RottenTomatoesContext.Top10' is null.");
        }

        public async Task<IActionResult> Top10()
        {
            // Redirigir al usuario a la vista de Loading
            await Task.WhenAll(ClearTop10Data(), add_top10());
            return RedirectToAction("Index");
        }
        public async Task ClearTop10Data()
        {
            var top10List = await _context.Top10.ToListAsync();
            _context.Top10.RemoveRange(top10List);
            await _context.SaveChangesAsync();
        }
        // GET: Top10/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Top10 == null)
            {
                return NotFound();
            }

            var top10 = await _context.Top10
                .FirstOrDefaultAsync(m => m.Top10Id == id);
            if (top10 == null)
            {
                return NotFound();
            }

            return View(top10);
        }

        // GET: Top10/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Top10/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Top10Id,title,url")] Top10 top10)
        {
            if (ModelState.IsValid)
            {
                _context.Add(top10);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(top10);
        }

        // GET: Top10/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Top10 == null)
            {
                return NotFound();
            }

            var top10 = await _context.Top10.FindAsync(id);
            if (top10 == null)
            {
                return NotFound();
            }
            return View(top10);
        }

        // POST: Top10/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Top10Id,title,url")] Top10 top10)
        {
            if (id != top10.Top10Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(top10);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Top10Exists(top10.Top10Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(top10);
        }

        // GET: Top10/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Top10 == null)
            {
                return NotFound();
            }

            var top10 = await _context.Top10
                .FirstOrDefaultAsync(m => m.Top10Id == id);
            if (top10 == null)
            {
                return NotFound();
            }

            return View(top10);
        }

        // POST: Top10/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Top10 == null)
            {
                return Problem("Entity set 'RottenTomatoesContext.Top10'  is null.");
            }
            var top10 = await _context.Top10.FindAsync(id);
            if (top10 != null)
            {
                _context.Top10.Remove(top10);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Top10Exists(int id)
        {
          return (_context.Top10?.Any(e => e.Top10Id == id)).GetValueOrDefault();
        }
    }
}
