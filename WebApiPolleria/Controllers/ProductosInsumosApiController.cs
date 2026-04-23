using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductosInsumosApiController : ControllerBase
{
    ProductoInsumoDAO _ProductosInsumos = new ProductoInsumoDAO();

    // RF-011: crear receta
    [HttpPost("registrar")]
    public async Task<string> registrar([FromBody] ProductoInsumo pi)
    {
        return await Task.Run(() =>
            _ProductosInsumos.insertar(pi)
        );
    }

    // RF-011: actualizar receta
    [HttpPut("actualizar")]
    public async Task<string> actualizar([FromBody] ProductoInsumo pi)
    {
        return await Task.Run(() =>
            _ProductosInsumos.actualizar(pi)
        );
    }

    // RF-011: eliminar receta
    [HttpDelete("eliminar/{id}")]
    public async Task<string> eliminar(int id)
    {
        return await Task.Run(() =>
            _ProductosInsumos.eliminar(id)
        );
    }

    // RF-011: listar receta por producto
    [HttpGet("listar/{idProducto}")]
    public async Task<IEnumerable<ProductoInsumo>> listar(int idProducto)
    {
        return await Task.Run(() =>
            _ProductosInsumos.listarPorProducto(idProducto)
        );
    }
}
