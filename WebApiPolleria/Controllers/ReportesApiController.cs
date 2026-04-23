using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

[ApiController]
[Route("api/[controller]")]
public class ReportesApiController : ControllerBase
{
    ReportesDAO _reportes = new ReportesDAO();

    // RF-035: Reporte de ventas por rango de fechas
    // GET: api/ReportesApi/ventas?inicio=2025-01-01&fin=2025-01-31
    [HttpGet("ventas")]
    public async Task<IEnumerable<ReporteVentas>> ventas(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fin
    )
    {
        return await Task.Run(() =>
            _reportes.ventas(inicio, fin)
        );
    }

    // RF-036: Productos más vendidos
    // GET: api/ReportesApi/productos?inicio=2025-01-01&fin=2025-01-31
    [HttpGet("productos")]
    public async Task<IEnumerable<ProductoMasVendido>> productos(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fin
    )
    {
        return await Task.Run(() =>
            _reportes.productosMasVendidos(inicio, fin)
        );
    }

    // RF-037: Consumo de insumos
    // GET: api/ReportesApi/consumo?inicio=2025-01-01&fin=2025-01-31
    [HttpGet("consumo")]
    public async Task<IEnumerable<ConsumoInsumo>> consumo(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fin
    )
    {
        return await Task.Run(() =>
            _reportes.consumo(inicio, fin)
        );
    }

    // RF-038: Stock actual con alertas
    // GET: api/ReportesApi/stock
    [HttpGet("stock")]
    public async Task<IEnumerable<StockReporte>> stock()
    {
        return await Task.Run(() =>
            _reportes.stock()
        );
    }

    // RF-039: Reporte por mesero
    // GET: api/ReportesApi/meseros?inicio=2025-01-01&fin=2025-01-31
    [HttpGet("meseros")]
    public async Task<IEnumerable<ReporteMesero>> meseros(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fin
    )
    {
        return await Task.Run(() =>
            _reportes.porMesero(inicio, fin)
        );
    }
}
