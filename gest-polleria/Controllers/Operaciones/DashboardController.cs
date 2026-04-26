using Microsoft.AspNetCore.Mvc;

namespace appPolleria.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
