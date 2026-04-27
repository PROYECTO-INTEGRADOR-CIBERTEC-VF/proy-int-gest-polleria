using Microsoft.AspNetCore.Mvc;

namespace appPolleria.Controllers
{
    public class MozoController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            return View();
        }
    }
}