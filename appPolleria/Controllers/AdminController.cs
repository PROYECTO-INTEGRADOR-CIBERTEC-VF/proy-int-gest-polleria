using Microsoft.AspNetCore.Mvc;

namespace appPolleria.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            return View();
        }
    }
}