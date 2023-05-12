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
    public class FavoriteSeriesController : Controller
    {
        private readonly RottenTomatoesContext _context;

        public FavoriteSeriesController(RottenTomatoesContext context)
        {
            _context = context;
        }

        // GET: FavoriteSeries
        public async Task<IActionResult> Index()
        {
            var cookie_userId = Request.Cookies["UserId"];
            int userId = int.Parse(cookie_userId);

            var favoriteSeries = await _context.FavoriteSerie
            .Include(f => f.Serie)
            .Include(f => f.User)
            .Where(f => f.UserId == userId) // Filtrar los favoritos del usuario actual
            .ToListAsync();
            return View(favoriteSeries);
        }

        // GET: FavoriteSeries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FavoriteSerie == null)
            {
                return NotFound();
            }

            var favoriteSerie = await _context.FavoriteSerie
                .Include(f => f.Serie)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteSerie == null)
            {
                return NotFound();
            }

            return View(favoriteSerie);
        }
        public async Task<IActionResult> AddToFavorites(int serieId)
        {
            var cookie_userId = Request.Cookies["UserId"];
            int userId = int.Parse(cookie_userId);

            var favoriteSerie = new FavoriteSerie(userId, serieId);

            await Create(favoriteSerie);
            // Guardar el objeto 'favorite' en la base de datos

            return RedirectToAction("Index");
        }
        // GET: FavoriteSeries/Create
        public IActionResult Create()
        {
            ViewData["SerieId"] = new SelectList(_context.Serie, "SerieId", "SerieId");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name");
            return View();
        }

        // POST: FavoriteSeries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,SerieId")] FavoriteSerie favoriteSerie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favoriteSerie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SerieId"] = new SelectList(_context.Serie, "SerieId", "SerieId", favoriteSerie.SerieId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", favoriteSerie.UserId);
            return View(favoriteSerie);
        }

        // GET: FavoriteSeries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FavoriteSerie == null)
            {
                return NotFound();
            }

            var favoriteSerie = await _context.FavoriteSerie.FindAsync(id);
            if (favoriteSerie == null)
            {
                return NotFound();
            }
            ViewData["SerieId"] = new SelectList(_context.Serie, "SerieId", "SerieId", favoriteSerie.SerieId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", favoriteSerie.UserId);
            return View(favoriteSerie);
        }

        // POST: FavoriteSeries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,SerieId")] FavoriteSerie favoriteSerie)
        {
            if (id != favoriteSerie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favoriteSerie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteSerieExists(favoriteSerie.Id))
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
            ViewData["SerieId"] = new SelectList(_context.Serie, "SerieId", "SerieId", favoriteSerie.SerieId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", favoriteSerie.UserId);
            return View(favoriteSerie);
        }

        // GET: FavoriteSeries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FavoriteSerie == null)
            {
                return NotFound();
            }

            var favoriteSerie = await _context.FavoriteSerie
                .Include(f => f.Serie)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteSerie == null)
            {
                return NotFound();
            }

            return View(favoriteSerie);
        }

        // POST: FavoriteSeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FavoriteSerie == null)
            {
                return Problem("Entity set 'RottenTomatoesContext.FavoriteSerie'  is null.");
            }
            var favoriteSerie = await _context.FavoriteSerie.FindAsync(id);
            if (favoriteSerie != null)
            {
                _context.FavoriteSerie.Remove(favoriteSerie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteSerieExists(int id)
        {
          return (_context.FavoriteSerie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
