using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class ClientesApiController : ControllerBase
    {
        ClienteDAO _cliente = new ClienteDAO();

        // GET: api/ClientesApi/listar?esEmpresa=true&buscar=...
        [HttpGet("listar")]
        public async Task<IEnumerable<Cliente>> listar([FromQuery] bool? esEmpresa = null, [FromQuery] string? buscar = null)
        {
            return await Task.Run(() => _cliente.listar(esEmpresa, buscar));
        }

        // GET: api/ClientesApi/buscar/5
        [HttpGet("buscar/{id}")]
        public async Task<Cliente?> buscar(int id)
        {
            return await Task.Run(() => _cliente.buscar(id));
        }

        // POST: api/ClientesApi/registrar
        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] Cliente c)
        {
            return await Task.Run(() =>
            {
                int idNuevo;
                string msg = _cliente.insertar(c, out idNuevo);
                return $"{msg} (IdClienteNuevo={idNuevo})";
            });
        }

        // PUT: api/ClientesApi/actualizar
        [HttpPut("actualizar")]
        public async Task<string> actualizar([FromBody] Cliente c)
        {
            return await Task.Run(() => _cliente.actualizar(c));
        }

        // DELETE: api/ClientesApi/desactivar/5
        [HttpDelete("desactivar/{id}")]
        public async Task<string> desactivar(int id)
        {
            return await Task.Run(() => _cliente.desactivar(id));
        }

        // PUT: api/ClientesApi/activar/5
        [HttpPut("activar/{id}")]
        public async Task<string> activar(int id)
        {
            return await Task.Run(() => _cliente.activar(id));
        }
    }
}
