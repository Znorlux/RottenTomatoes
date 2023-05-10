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
    public class FavoriteMoviesController : Controller
    {
        private readonly RottenTomatoesContext _context;

        public FavoriteMoviesController(RottenTomatoesContext context)
        {
            _context = context;
        }

        // GET: FavoriteMovies
        public async Task<IActionResult> Index()
        {
            var rottenTomatoesContext = _context.FavoriteMovie.Include(f => f.Movie).Include(f => f.User);
            return View(await rottenTomatoesContext.ToListAsync());
        }

        // GET: FavoriteMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FavoriteMovie == null)
            {
                return NotFound();
            }

            var favoriteMovie = await _context.FavoriteMovie
                .Include(f => f.Movie)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteMovie == null)
            {
                return NotFound();
            }

            return View(favoriteMovie);
        }

        // GET: FavoriteMovies/Create
        public async Task<IActionResult> AddToFavorites(int movieId)
        {
            var cookie_userId = Request.Cookies["UserId"];
            int userId = int.Parse(cookie_userId);

            var favoriteMovie = new FavoriteMovie(userId, movieId);
            
            await Create(favoriteMovie);
            // Guardar el objeto 'favorite' en la base de datos

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "MovieId");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name");
            return View();
        }

        // POST: FavoriteMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,MovieId")] FavoriteMovie favoriteMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favoriteMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "MovieId", favoriteMovie.MovieId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", favoriteMovie.UserId);
            return View(favoriteMovie);
        }

        // GET: FavoriteMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FavoriteMovie == null)
            {
                return NotFound();
            }

            var favoriteMovie = await _context.FavoriteMovie.FindAsync(id);
            if (favoriteMovie == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "MovieId", favoriteMovie.MovieId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", favoriteMovie.UserId);
            return View(favoriteMovie);
        }

        // POST: FavoriteMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,MovieId")] FavoriteMovie favoriteMovie)
        {
            if (id != favoriteMovie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favoriteMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteMovieExists(favoriteMovie.Id))
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
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "MovieId", favoriteMovie.MovieId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", favoriteMovie.UserId);
            return View(favoriteMovie);
        }

        // GET: FavoriteMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FavoriteMovie == null)
            {
                return NotFound();
            }

            var favoriteMovie = await _context.FavoriteMovie
                .Include(f => f.Movie)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteMovie == null)
            {
                return NotFound();
            }

            return View(favoriteMovie);
        }

        // POST: FavoriteMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FavoriteMovie == null)
            {
                return Problem("Entity set 'RottenTomatoesContext.FavoriteMovie'  is null.");
            }
            var favoriteMovie = await _context.FavoriteMovie.FindAsync(id);
            if (favoriteMovie != null)
            {
                _context.FavoriteMovie.Remove(favoriteMovie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteMovieExists(int id)
        {
          return (_context.FavoriteMovie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
