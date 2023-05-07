using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RottenTomatoes.Data;
using RottenTomatoes.Models;

namespace RottenTomatoes.Controllers
{
    public class UsersController : Controller
    {
        private readonly RottenTomatoesContext _context;

        public UsersController(RottenTomatoesContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'RottenTomatoesContext.User'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Password,UserType")] User user)
        {
            if (user.UserType == "0")
            {
                ViewBag.Error = "Ingrese un tipo de usuario válido.";
                return View(user);
            }
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Name == user.Name);//Comparamos el usuario que ingresó
                                                                                            //con todos los usuarios
            if (existingUser != null)//Si el usuario que ingresó ya existia, se creará un objeto  
            {                        //de esta manera, sabremos que existe un usuario con ese usuario ingresado y no se podrá registrar
                ViewBag.Error = "El nombre de usuario ya existe. Por favor ingrese uno diferente.";
                return View(user);
            }

            if (ModelState.IsValid)//Si todo sale bien, se añade a la base de datos 
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                ViewBag.Success = true;
                return View(user);
                //return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Name == username && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "El nombre de usuario o la contraseña son incorrectos.";
                return View("Login");
            }
            if(user.UserType == "Experto en cine")
            {
                return RedirectToAction("RTscrapper", "Home");//Aqui lo mandaremos a la pagina del web scrapper
                                                            
            }else if (user.UserType == "Cinefilo")
            {
                return RedirectToAction("Index","Movies");//Aqui lo mandaremos a la ruta de vista de peliculas y todo
                                                       //Archivo  Carpeta (vista, controlador)
            }
            return RedirectToAction(nameof(Index));
        }



        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Password,UserType")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'RottenTomatoesContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
