using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using gest_polleria_front.Filters;
using gest_polleria_front.Models;
using gest_polleria_front.Services;

namespace gest_polleria_front.Controllers
{
    [SessionAuthorize(RequiredRole = "Administrador")]
    public class InsumosController : AppController
    {
        private readonly PolleriaApiService _api = new PolleriaApiService();

        [HttpGet]
        public async Task<ActionResult> Index(bool mostrarAlertas = false, decimal porcentaje = 10)
        {
            return View(await BuildViewModelAsync(new InsumoFormViewModel(), mostrarAlertas, porcentaje));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(InsumoFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await BuildViewModelAsync(model, false, 10));
            }

            try
            {
                await _api.CreateInsumoAsync(model);
                FlashSuccess("Insumo registrado correctamente.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos registrar el insumo. Revisa los datos e inténtalo nuevamente.");
                return View("Index", await BuildViewModelAsync(model, false, 10));
            }
        }

        private async Task<InsumosIndexViewModel> BuildViewModelAsync(InsumoFormViewModel form, bool mostrarAlertas, decimal porcentaje)
        {
            var model = new InsumosIndexViewModel
            {
                MostrarAlertas = mostrarAlertas,
                PorcentajeAlerta = porcentaje <= 0 ? 10 : porcentaje,
                NuevoInsumo = form ?? new InsumoFormViewModel()
            };

            try
            {
                model.StockItems = mostrarAlertas
                    ? await _api.GetAlertasStockAsync(model.PorcentajeAlerta)
                    : await _api.GetReporteStockAsync();
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar la información de existencias. Inténtalo nuevamente.");
            }

            return model;
        }
    }
}
