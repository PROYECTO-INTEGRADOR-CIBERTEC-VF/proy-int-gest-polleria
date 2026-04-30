using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using gest_polleria_front.Filters;
using gest_polleria_front.Models;
using gest_polleria_front.Services;

namespace gest_polleria_front.Controllers
{
    [SessionAuthorize(RequiredRole = "Administrador")]
    public class MeserosController : AppController
    {
        private readonly PolleriaApiService _api = new PolleriaApiService();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await BuildViewModelAsync(new MeseroFormViewModel(), new MeseroTurnoFormViewModel(), new MeseroZonaFormViewModel()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(MeseroFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveForm = "crearMesero";
                return View("Index", await BuildViewModelAsync(model, new MeseroTurnoFormViewModel(), new MeseroZonaFormViewModel()));
            }

            try
            {
                await _api.CreateMeseroAsync(model);
                FlashSuccess("Mesero registrado correctamente.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos registrar al mesero. Revisa los datos e inténtalo nuevamente.");
                ViewBag.ActiveForm = "crearMesero";
                return View("Index", await BuildViewModelAsync(model, new MeseroTurnoFormViewModel(), new MeseroZonaFormViewModel()));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegistrarTurno(MeseroTurnoFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveForm = "turnoMesero";
                return View("Index", await BuildViewModelAsync(new MeseroFormViewModel(), model, new MeseroZonaFormViewModel()));
            }

            try
            {
                await _api.RegistrarTurnoAsync(model);
                FlashSuccess("Turno guardado correctamente.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos guardar el turno. Inténtalo nuevamente.");
                ViewBag.ActiveForm = "turnoMesero";
                return View("Index", await BuildViewModelAsync(new MeseroFormViewModel(), model, new MeseroZonaFormViewModel()));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AsignarZona(MeseroZonaFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveForm = "zonaMesero";
                return View("Index", await BuildViewModelAsync(new MeseroFormViewModel(), new MeseroTurnoFormViewModel(), model));
            }

            try
            {
                await _api.AsignarZonaAsync(model);
                FlashSuccess("Zona asignada correctamente.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos asignar la zona. Inténtalo nuevamente.");
                ViewBag.ActiveForm = "zonaMesero";
                return View("Index", await BuildViewModelAsync(new MeseroFormViewModel(), new MeseroTurnoFormViewModel(), model));
            }
        }

        private async Task<MeserosIndexViewModel> BuildViewModelAsync(
            MeseroFormViewModel mesero,
            MeseroTurnoFormViewModel turno,
            MeseroZonaFormViewModel zona)
        {
            var model = new MeserosIndexViewModel
            {
                NuevoMesero = mesero ?? new MeseroFormViewModel(),
                NuevoTurno = turno ?? new MeseroTurnoFormViewModel(),
                NuevaZona = zona ?? new MeseroZonaFormViewModel()
            };

            try
            {
                model.Meseros = await _api.GetMeserosAsync();
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar el personal. Inténtalo nuevamente.");
            }

            return model;
        }
    }
}
