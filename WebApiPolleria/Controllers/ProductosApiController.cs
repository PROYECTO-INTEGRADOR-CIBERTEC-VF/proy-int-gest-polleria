using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class ProductosApiController : ControllerBase
    {
        ProductoDAO _producto = new ProductoDAO();

        // GET: api/ProductosApi/listar?soloActivos=true&idCategoria=1&buscar=...
        [HttpGet("listar")]
        public async Task<IEnumerable<Producto>> listar([FromQuery] bool soloActivos = true, [FromQuery] int? idCategoria = null, [FromQuery] string? buscar = null)
        {
            return await Task.Run(() => _producto.listar(soloActivos, idCategoria, buscar));
        }

        // GET: api/ProductosApi/buscar/5
        [HttpGet("buscar/{id}")]
        public async Task<Producto?> buscar(int id)
        {
            return await Task.Run(() => _producto.buscar(id));
        }

        // POST: api/ProductosApi/registrar
        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] Producto p)
        {
            return await Task.Run(() =>
            {
                int idNuevo;
                string msg = _producto.insertar(p, out idNuevo);
                return $"{msg} (IdProductoNuevo={idNuevo})";
            });
        }

        // PUT: api/ProductosApi/actualizar
        [HttpPut("actualizar")]
        public async Task<string> actualizar([FromBody] Producto p)
        {
            return await Task.Run(() => _producto.actualizar(p));
        }

        // DELETE: api/ProductosApi/desactivar/5
        [HttpDelete("desactivar/{id}")]
        public async Task<string> desactivar(int id)
        {
            return await Task.Run(() => _producto.desactivar(id));
        }

        // PUT: api/ProductosApi/activar/5
        [HttpPut("activar/{id}")]
        public async Task<string> activar(int id)
        {
            return await Task.Run(() => _producto.activar(id));
        }

    }
}
