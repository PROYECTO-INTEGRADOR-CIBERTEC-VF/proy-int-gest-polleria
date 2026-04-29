using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using gest_polleria_front.Filters;
using gest_polleria_front.Models;
using gest_polleria_front.Services;

namespace gest_polleria_front.Controllers
{
    [SessionAuthorize(DeniedRole = "MESERO")]
    public class ReportesController : AppController
    {
        private readonly PolleriaApiService _api = new PolleriaApiService();

        [HttpGet]
        public async Task<ActionResult> Index(string tipo = "diario", DateTime? fecha = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var model = new ReportesIndexViewModel
            {
                TipoConsulta = string.Equals(tipo, "rango", StringComparison.OrdinalIgnoreCase) ? "rango" : "diario",
                Fecha = fecha ?? DateTime.Today,
                FechaInicio = fechaInicio ?? DateTime.Today,
                FechaFin = fechaFin ?? DateTime.Today
            };

            try
            {
                model.Resultados = model.TipoConsulta == "rango"
                    ? await _api.GetDemandaRangoAsync(model.FechaInicio, model.FechaFin)
                    : await _api.GetDemandaDiariaAsync(model.Fecha);

                model.TotalPedidos = model.Resultados.Sum(item => item.CantidadPedidos);
                model.TotalEstimado = model.Resultados.Sum(item => item.TotalEstimado);
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar el reporte. Inténtalo nuevamente.");
            }

            return View(model);
        }
    }
}
