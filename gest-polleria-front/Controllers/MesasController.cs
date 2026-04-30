using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using gest_polleria_front.Filters;
using gest_polleria_front.Models;
using gest_polleria_front.Services;

namespace gest_polleria_front.Controllers
{
    [SessionAuthorize(DeniedRole = "MESERO")]
    public class MesasController : AppController
    {
        private readonly PolleriaApiService _api = new PolleriaApiService();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await BuildViewModelAsync(new MesaFormViewModel(), new EstadoMesaViewModel()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(MesaFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveForm = "crearMesa";
                return View("Index", await BuildViewModelAsync(model, new EstadoMesaViewModel()));
            }

            try
            {
                await _api.CreateMesaAsync(model);
                FlashSuccess("Mesa registrada correctamente.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos registrar la mesa. Revisa los datos e inténtalo nuevamente.");
                ViewBag.ActiveForm = "crearMesa";
                return View("Index", await BuildViewModelAsync(model, new EstadoMesaViewModel()));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CambiarEstado(EstadoMesaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveForm = "estadoMesa";
                return View("Index", await BuildViewModelAsync(new MesaFormViewModel(), model));
            }

            try
            {
                await _api.ChangeMesaStateAsync(model);
                FlashSuccess("Estado de mesa actualizado correctamente.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos actualizar el estado de la mesa. Inténtalo nuevamente.");
                ViewBag.ActiveForm = "estadoMesa";
                return View("Index", await BuildViewModelAsync(new MesaFormViewModel(), model));
            }
        }

        private async Task<MesasIndexViewModel> BuildViewModelAsync(MesaFormViewModel nuevaMesa, EstadoMesaViewModel cambioEstado)
        {
            var model = new MesasIndexViewModel
            {
                NuevaMesa = nuevaMesa ?? new MesaFormViewModel(),
                CambioEstado = cambioEstado ?? new EstadoMesaViewModel()
            };

            try
            {
                model.Mesas = await _api.GetMesasAsync(true);
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar las mesas. Inténtalo nuevamente.");
            }

            return model;
        }
    }
}
