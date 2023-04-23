using Microsoft.AspNetCore.Mvc;

namespace RottenTomatoes.Controllers
{
    //Aqui solo creamos un Controlador el cual hereda de la clase Controller proveniente del framework mvc
    public class AccountController : Controller
    {
        public ActionResult Login()//Este metodo debe llamarse tal cual como el archivo .cshtml del cual se cargará la pagina
        {
            //Este método de acción View() es
            //responsable de buscar y devolver una vista (en este caso, la vista Login.cshtml).
            return View();
        }
    }
}
