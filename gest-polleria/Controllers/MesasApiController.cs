using gest_polleria.DAO;
using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;

namespace gest_polleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesasApiController : ControllerBase
    {
        private readonly MesaDAO _mesa = new MesaDAO();

        [HttpGet("listar")]
        public async Task<IEnumerable<Mesa>> listar([FromQuery] bool soloActivas = true)
        {
            return await Task.Run(() => _mesa.listar(soloActivas));
        }

        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] Mesa mesa)
        {
            return await Task.Run(() =>
            {
                int idNuevo;
                return _mesa.insertar(mesa, out idNuevo);
            });
        }

        [HttpPut("actualizar")]
        public async Task<string> actualizar([FromBody] Mesa mesa)
        {
            return await Task.Run(() => _mesa.actualizar(mesa));
        }

        [HttpDelete("desactivar/{id}")]
        public async Task<string> desactivar(int id)
        {
            return await Task.Run(() => _mesa.desactivar(id));
        }

        [HttpPut("estado")]
        public async Task<string> cambiarEstado([FromBody] EstadoMesaRequest request)
        {
            return await Task.Run(() => _mesa.cambiarEstado(request.IdMesa, request.Estado));
        }
    }
}
