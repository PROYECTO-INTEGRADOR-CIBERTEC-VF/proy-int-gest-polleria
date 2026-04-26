using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class MeseroApiController : ControllerBase
    {
        MeseroDAO _mesero = new MeseroDAO();

        // GET: api/MeseroApi/listar
        [HttpGet("listar")]
        public async Task<IEnumerable<Mesero>> listar()
        {
            return await Task.Run(() => _mesero.listar());
        }

        // GET: api/MeseroApi/buscar/5
        [HttpGet("buscar/{id}")]
        public async Task<Mesero?> buscar(int id)
        {
            return await Task.Run(() => _mesero.buscar(id));
        }

        // POST: api/MeseroApi/registrar
        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] Mesero m)
        {
            return await Task.Run(() =>
            {
                // Si tu DAO de mesero no usa 'out', simplemente lo llamamos así:
                bool ok = _mesero.registrar(m);
                return ok ? "Mesero registrado correctamente" : "Error al registrar mesero";
            });
        }

        // PUT: api/MeseroApi/actualizar
        [HttpPut("actualizar")]
        public async Task<string> actualizar([FromBody] Mesero m)
        {
            return await Task.Run(() =>
            {
                bool ok = _mesero.actualizar(m);
                return ok ? "Mesero actualizado correctamente" : "Error al actualizar";
            });
        }

        // DELETE: api/MeseroApi/desactivar/5
        [HttpDelete("desactivar/{id}")]
        public async Task<string> desactivar(int id)
        {
            return await Task.Run(() =>
            {
                _mesero.cambiarEstado(id, false);
                return "Mesero desactivado correctamente";
            });
        }

        // PUT: api/MeseroApi/activar/5
        [HttpPut("activar/{id}")]
        public async Task<string> activar(int id)
        {
            return await Task.Run(() =>
            {
                _mesero.cambiarEstado(id, true);
                return "Mesero activado correctamente";
            });
        }
    }
}