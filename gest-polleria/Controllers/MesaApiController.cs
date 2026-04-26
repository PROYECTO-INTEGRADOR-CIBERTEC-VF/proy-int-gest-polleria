using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gest_polleria.Controllers
{
    public class MesaApiController : Controller
    {
        // GET: MesaApiController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MesaApiController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MesaApiController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MesaApiController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MesaApiController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MesaApiController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MesaApiController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MesaApiController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}