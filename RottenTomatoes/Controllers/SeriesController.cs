using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RottenTomatoes.Data;
using RottenTomatoes.Models;

namespace RottenTomatoes.Controllers
{
    public class SeriesController : Controller
    {
        private readonly RottenTomatoesContext _context;

        public SeriesController(RottenTomatoesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> FilterSeriesDown()
        {
            var model = _context.Serie.OrderBy(m => Convert.ToInt32(m.TomatometerScore));
            ViewBag.Filter = true;
            ViewBag.FilterDown = "Worst tomatometer score series";
            return View("Index", model);
        }
        public async Task<IActionResult> FilterSeriesUp()
        {
            var model = _context.Serie.OrderByDescending(m => Convert.ToInt32(m.TomatometerScore));
            ViewBag.Filter = true;
            ViewBag.FilterUp = "Best tomatometer score movies";
            return View("Index", model);
        }

        public IActionResult Index()
        {
            bool filter = false;
            if (ViewBag.Filter != null)
            {
                filter = ViewBag.Filter;
            }

            if (filter)
            {
                return View((List<Serie>)ViewBag.Model);
            }
            else
            {
                var model = _context.Serie.ToList();
                return View(model);
            }
        }

        // GET: Series/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Serie == null)
            {
                return NotFound();
            }

            var serie = await _context.Serie
                .FirstOrDefaultAsync(m => m.SerieId == id);
            if (serie == null)
            {
                return NotFound();
            }

            return View(serie);
        }

        // GET: Series/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Series/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SerieId,Creator,mainActors,Title,ImageURL,TomatometerScore,AudienceScore,Platforms,Synopsis,Genre,ReleaseDate")] Serie serie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serie);
        }

        // GET: Series/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Serie == null)
            {
                return NotFound();
            }

            var serie = await _context.Serie.FindAsync(id);
            if (serie == null)
            {
                return NotFound();
            }
            return View(serie);
        }

        // POST: Series/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SerieId,Creator,mainActors,Title,ImageURL,TomatometerScore,AudienceScore,Platforms,Synopsis,Genre,ReleaseDate")] Serie serie)
        {
            if (id != serie.SerieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SerieExists(serie.SerieId))
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
            return View(serie);
        }

        // GET: Series/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Serie == null)
            {
                return NotFound();
            }

            var serie = await _context.Serie
                .FirstOrDefaultAsync(m => m.SerieId == id);
            if (serie == null)
            {
                return NotFound();
            }

            return View(serie);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Serie == null)
            {
                return Problem("Entity set 'RottenTomatoesContext.Serie'  is null.");
            }
            var serie = await _context.Serie.FindAsync(id);
            if (serie != null)
            {
                _context.Serie.Remove(serie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SerieExists(int id)
        {
          return (_context.Serie?.Any(e => e.SerieId == id)).GetValueOrDefault();
        }
    }
}
