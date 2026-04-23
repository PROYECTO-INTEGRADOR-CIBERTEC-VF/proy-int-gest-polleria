using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class ProveedoresApiController : ControllerBase
    {
        ProveedorDAO _proveedor = new ProveedorDAO();

        // GET: api/ProveedoresApi/listar?soloActivos=true&buscar=...
        [HttpGet("listar")]
        public async Task<IEnumerable<Proveedor>> listar([FromQuery] bool soloActivos = true, [FromQuery] string? buscar = null)
        {
            return await Task.Run(() => _proveedor.listar(soloActivos, buscar));
        }

        // GET: api/ProveedoresApi/buscar/5
        [HttpGet("buscar/{id}")]
        public async Task<Proveedor?> buscar(int id)
        {
            return await Task.Run(() => _proveedor.buscar(id));
        }

        // POST: api/ProveedoresApi/registrar
        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] Proveedor p)
        {
            return await Task.Run(() =>
            {
                int idNuevo;
                string msg = _proveedor.insertar(p, out idNuevo);
                return $"{msg} (IdProveedorNuevo={idNuevo})";
            });
        }

        // PUT: api/ProveedoresApi/actualizar
        [HttpPut("actualizar")]
        public async Task<string> actualizar([FromBody] Proveedor p)
        {
            return await Task.Run(() => _proveedor.actualizar(p));
        }

        // DELETE: api/ProveedoresApi/desactivar/5
        [HttpDelete("desactivar/{id}")]
        public async Task<string> desactivar(int id)
        {
            return await Task.Run(() => _proveedor.desactivar(id));
        }

        // PUT: api/ProveedoresApi/activar/5
        [HttpPut("activar/{id}")]
        public async Task<string> activar(int id)
        {
            return await Task.Run(() => _proveedor.activar(id));
        }

    }
}
