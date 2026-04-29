using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using gest_polleria_front.Filters;
using gest_polleria_front.Models;
using gest_polleria_front.Services;

namespace gest_polleria_front.Controllers
{
    [SessionAuthorize(DeniedRole = "MESERO")]
    public class ClientesController : AppController
    {
        private readonly PolleriaApiService _api = new PolleriaApiService();

        [HttpGet]
        public async Task<ActionResult> Index(string buscar = "", bool? esEmpresa = null)
        {
            return View(await BuildViewModelAsync(new ClienteFormViewModel(), buscar, esEmpresa));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(ClienteFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await BuildViewModelAsync(model, string.Empty, null));
            }

            try
            {
                await _api.CreateClienteAsync(model);
                FlashSuccess("Cliente registrado correctamente.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos registrar el cliente. Revisa los datos e inténtalo nuevamente.");
                return View("Index", await BuildViewModelAsync(model, string.Empty, null));
            }
        }

        private async Task<ClientesIndexViewModel> BuildViewModelAsync(ClienteFormViewModel form, string buscar, bool? esEmpresa)
        {
            var model = new ClientesIndexViewModel
            {
                Buscar = buscar ?? string.Empty,
                EsEmpresa = esEmpresa,
                NuevoCliente = form ?? new ClienteFormViewModel()
            };

            try
            {
                model.Clientes = await _api.GetClientesAsync(esEmpresa, buscar);
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar los clientes. Inténtalo nuevamente.");
            }

            return model;
        }
    }
}
