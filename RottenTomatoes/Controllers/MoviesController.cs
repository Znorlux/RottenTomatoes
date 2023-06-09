﻿using System;
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
    public class MoviesController : Controller
    {
        private readonly RottenTomatoesContext _context;

        public MoviesController(RottenTomatoesContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> FilterMoviesDown()
        {
            var model = _context.Movie.OrderBy(m => Convert.ToInt32(m.TomatometerScore));
            ViewBag.Filter = true;
            ViewBag.FilterDown = "Worst tomatometer score movies";
            return View("Index", model);
        }
        public async Task<IActionResult> FilterMoviesUp()
        {
            var model = _context.Movie.OrderByDescending(m => Convert.ToInt32(m.TomatometerScore));
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
                return View((List<Movie>)ViewBag.Model);
            }
            else
            {
                var model = _context.Movie.ToList();
                return View(model);
            }
        }
        public async Task ClearTop10Data()
        {
            var MovieList = await _context.Movie.ToListAsync();
            _context.Movie.RemoveRange(MovieList);
            await _context.SaveChangesAsync();
        }
        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,CriticReview,AudienceReview,Clasification,OriginalLanguage,Director,Runtime,ActorRoles,Title,ImageURL,TomatometerScore,AudienceScore,Platforms,Synopsis,Genre,ReleaseDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,CriticReview,AudienceReview,Clasification,OriginalLanguage,Director,Runtime,ActorRoles,Title,ImageURL,TomatometerScore,AudienceScore,Platforms,Synopsis,Genre,ReleaseDate")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'RottenTomatoesContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movie?.Any(e => e.MovieId == id)).GetValueOrDefault();
        }
    }
}
