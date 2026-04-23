using Microsoft.AspNetCore.Mvc;

namespace gest_polleria.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
