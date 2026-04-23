using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class InsumosApiController : ControllerBase
    {
        InsumoDAO _insumo = new InsumoDAO();

        // GET: api/InsumosApi/listar?soloActivos=true&idUnidadMedida=1&buscar=...
        [HttpGet("listar")]
        public async Task<IEnumerable<Insumo>> listar([FromQuery] bool soloActivos = true, [FromQuery] int? idUnidadMedida = null, [FromQuery] string? buscar = null)
        {
            return await Task.Run(() => _insumo.listar(soloActivos, idUnidadMedida, buscar));
        }

        // GET: api/InsumosApi/buscar/5
        [HttpGet("buscar/{id}")]
        public async Task<Insumo?> buscar(int id)
        {
            return await Task.Run(() => _insumo.buscar(id));
        }

        // POST: api/InsumosApi/registrar
        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] Insumo i)
        {
            return await Task.Run(() =>
            {
                int idNuevo;
                string msg = _insumo.insertar(i, out idNuevo);
                return $"{msg} (IdInsumoNuevo={idNuevo})";
            });
        }

        // PUT: api/InsumosApi/actualizar
        [HttpPut("actualizar")]
        public async Task<string> actualizar([FromBody] Insumo i)
        {
            return await Task.Run(() => _insumo.actualizar(i));
        }

        // DELETE: api/InsumosApi/desactivar/5
        [HttpDelete("desactivar/{id}")]
        public async Task<string> desactivar(int id)
        {
            return await Task.Run(() => _insumo.desactivar(id));
        }

        // PUT: api/InsumosApi/activar/5
        [HttpPut("activar/{id}")]
        public async Task<string> activar(int id)
        {
            return await Task.Run(() => _insumo.activar(id));
        }
 
    }
}
