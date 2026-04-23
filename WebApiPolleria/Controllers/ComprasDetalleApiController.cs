using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

[Route("api/[controller]")]
[ApiController]
public class ComprasDetalleApiController : ControllerBase
{
    CompraDetalleDAO _detalle = new CompraDetalleDAO();

    // RF-009: agregar detalle de compra
    // POST: api/ComprasDetalleApi/agregar
    [HttpPost("agregar")]
    public async Task<string> agregar([FromBody] CompraDetalle d)
    {
        return await Task.Run(() =>
            _detalle.agregarDetalle(d)
        );
    }
}
