using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class MesasApiController : ControllerBase
    {
        MesaDAO _mesa = new MesaDAO();

        // GET: api/MesasApi/listar?soloActivas=true&buscar=...
        [HttpGet("listar")]
        public async Task<IEnumerable<Mesa>> listar([FromQuery] bool soloActivas = true, [FromQuery] string? buscar = null)
        {
            return await Task.Run(() => _mesa.listar(soloActivas, buscar));
        }

        // GET: api/MesasApi/buscar/5
        [HttpGet("buscar/{id}")]
        public async Task<Mesa?> buscar(int id)
        {
            return await Task.Run(() => _mesa.buscar(id));
        }

        // POST: api/MesasApi/registrar
        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] Mesa m)
        {
            return await Task.Run(() =>
            {
                int idNuevo;
                string msg = _mesa.insertar(m, out idNuevo);
                return $"{msg} (IdMesaNueva={idNuevo})";
            });
        }

        // PUT: api/MesasApi/actualizar
        [HttpPut("actualizar")]
        public async Task<string> actualizar([FromBody] Mesa m)
        {
            return await Task.Run(() => _mesa.actualizar(m));
        }

        // DELETE: api/MesasApi/desactivar/5
        [HttpDelete("desactivar/{id}")]
        public async Task<string> desactivar(int id)
        {
            return await Task.Run(() => _mesa.desactivar(id));
        }

        // PUT: api/MesasApi/activar/5
        [HttpPut("activar/{id}")]
        public async Task<string> activar(int id)
        {
            return await Task.Run(() => _mesa.activar(id));
        }

    }
}
