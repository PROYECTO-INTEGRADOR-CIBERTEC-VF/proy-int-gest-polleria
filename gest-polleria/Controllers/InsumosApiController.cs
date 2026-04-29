using gest_polleria.DAO;
using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;

namespace gest_polleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsumosApiController : ControllerBase
    {
        private readonly InsumoDAO _insumo = new InsumoDAO();

        [HttpGet("listar")]
        public async Task<IEnumerable<Insumo>> listar([FromQuery] bool soloActivos = true, [FromQuery] string? buscar = null)
        {
            return await Task.Run(() => _insumo.listar(soloActivos, buscar));
        }

        [HttpGet("buscar/{id}")]
        public async Task<Insumo?> buscar(int id)
        {
            return await Task.Run(() => _insumo.buscar(id));
        }

        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] Insumo i)
        {
            return await Task.Run(() =>
            {
                int idNuevo;
                return _insumo.insertar(i, out idNuevo);
            });
        }

        [HttpPut("actualizar")]
        public async Task<string> actualizar([FromBody] Insumo i)
        {
            return await Task.Run(() => _insumo.actualizar(i));
        }

        [HttpDelete("desactivar/{id}")]
        public async Task<string> desactivar(int id)
        {
            return await Task.Run(() => _insumo.desactivar(id));
        }

        [HttpPost("ajustar-stock")]
        public async Task<string> ajustarStock([FromBody] AjusteInventario ajuste)
        {
            return await Task.Run(() => _insumo.ajustarStock(ajuste));
        }

        [HttpGet("alertas")]
        public async Task<IEnumerable<StockReporte>> alertas([FromQuery] decimal porcentaje = 10)
        {
            return await Task.Run(() => _insumo.alertas(porcentaje));
        }

        [HttpGet("reporte-stock")]
        public async Task<IEnumerable<StockReporte>> reporteStock()
        {
            return await Task.Run(() => _insumo.reporteStock());
        }
    }
}
