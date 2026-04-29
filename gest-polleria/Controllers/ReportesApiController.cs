using gest_polleria.DAO;
using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;

namespace gest_polleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesApiController : ControllerBase
    {
        private readonly ReporteDAO _reporte = new ReporteDAO();

        [HttpGet("demanda-diaria")]
        public async Task<IEnumerable<DemandaDiariaReporte>> demandaDiaria([FromQuery] DateTime? fecha = null)
        {
            var f = fecha ?? DateTime.Today;
            return await Task.Run(() => _reporte.demandaDiaria(f));
        }

        [HttpGet("demanda-rango")]
        public async Task<IEnumerable<DemandaDiariaReporte>> demandaRango([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            return await Task.Run(() => _reporte.demandaRango(fechaInicio, fechaFin));
        }
    }
}
