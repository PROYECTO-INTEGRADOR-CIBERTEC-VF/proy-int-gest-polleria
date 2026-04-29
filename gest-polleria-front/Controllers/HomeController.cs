using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using gest_polleria_front.Filters;
using gest_polleria_front.Models;
using gest_polleria_front.Services;

namespace gest_polleria_front.Controllers
{
    public class HomeController : AppController
    {
        private readonly PolleriaApiService _api = new PolleriaApiService();

        [SessionAuthorize]
        public async Task<ActionResult> Index()
        {
            if (IsMesero)
            {
                return RedirectToAction("Mesero", "Pedidos");
            }

            var model = new DashboardViewModel
            {
                UserName = CurrentUserName,
                NombreCompleto = CurrentUserDisplayName,
                Role = CurrentUserRole
            };

            try
            {
                var clientesTask = _api.GetClientesAsync(null, string.Empty);
                var mesasTask = _api.GetMesasAsync(true);
                var meserosTask = _api.GetMeserosAsync();
                var stockTask = _api.GetReporteStockAsync();
                var reportesTask = _api.GetDemandaDiariaAsync(DateTime.Today);

                await Task.WhenAll(clientesTask, mesasTask, meserosTask, stockTask, reportesTask);

                var clientes = clientesTask.Result ?? new List<ClienteDto>();
                var mesas = mesasTask.Result ?? new List<MesaDto>();
                var meseros = meserosTask.Result ?? new List<MeseroDto>();
                var stock = stockTask.Result ?? new List<StockReporteDto>();
                var reportes = reportesTask.Result ?? new List<DemandaDiariaReporteDto>();

                model.TotalClientes = clientes.Count;
                model.TotalMesas = mesas.Count;
                model.MesasLibres = mesas.Count(m => string.Equals(m.Estado, "LIBRE", StringComparison.OrdinalIgnoreCase));
                model.MesasOcupadas = mesas.Count - model.MesasLibres;
                model.TotalMeserosActivos = meseros.Count(m => m.Activo);
                model.AlertasStock = stock
                    .Where(s => !string.Equals(s.EstadoStock, "OK", StringComparison.OrdinalIgnoreCase))
                    .Take(5)
                    .ToList();
                model.TotalAlertasStock = model.AlertasStock.Count;
                model.Mesas = mesas.Take(6).ToList();
                model.ReporteDiario = reportes.Take(6).ToList();
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar el resumen. Inténtalo nuevamente.");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (IsAuthenticated)
            {
                return IsMesero
                    ? RedirectToAction("Mesero", "Pedidos")
                    : RedirectToAction("Index");
            }

            return View(new LoginPageViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var auth = await _api.LoginAsync(model.UserName, model.ClaveHash);
                if (auth == null || !auth.Ok)
                {
                    ModelState.AddModelError("", "Usuario o clave incorrectos.");
                    return View(model);
                }

                SignIn(auth, model.UserName);
                FlashSuccess("Sesión iniciada correctamente.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos iniciar sesión. Inténtalo nuevamente.");
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            SignOutUser();
            return RedirectToAction("Login");
        }
    }
}
