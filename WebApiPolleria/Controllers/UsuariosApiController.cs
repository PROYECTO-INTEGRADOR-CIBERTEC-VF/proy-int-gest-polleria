using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class UsuariosApiController : ControllerBase
    {
        UsuarioDAO _usuario = new UsuarioDAO();

        // GET: api/UsuariosApi/listar?soloActivos=true&buscar=...
        [HttpGet("listar")]
        public async Task<IEnumerable<Usuario>> listar([FromQuery] bool soloActivos = true, [FromQuery] string? buscar = null)
        {
            return await Task.Run(() => _usuario.listar(soloActivos, buscar));
        }

        // GET: api/UsuariosApi/buscar/5
        [HttpGet("buscar/{id}")]
        public async Task<Usuario?> buscar(int id)
        {
            return await Task.Run(() => _usuario.buscar(id));
        }

        // POST: api/UsuariosApi/registrar
        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] UsuarioRequest req)
        {
            return await Task.Run(() =>
            {
                int idNuevo;
                var msg = _usuario.insertarDesdeRequest(req, out idNuevo);
                return $"{msg} (IdUsuarioNuevo={idNuevo})";
            });
        }



        // PUT: api/UsuariosApi/actualizar
        [HttpPut("actualizar")]
        public async Task<string> actualizar([FromBody] UsuarioActualizarRequest req)
        {
            return await Task.Run(() => _usuario.actualizarDesdeRequest(req));
        }


        // DELETE: api/UsuariosApi/desactivar/5
        [HttpDelete("desactivar/{id}")]
        public async Task<string> desactivar(int id)
        {
            return await Task.Run(() => _usuario.desactivar(id));
        }

        // PUT: api/UsuariosApi/activar/5
        [HttpPut("activar/{id}")]
        public async Task<string> activar(int id)
        {
            return await Task.Run(() => _usuario.activar(id));
        }

        // PUT: api/UsuariosApi/cambiarclave
        [HttpPut("cambiarclave")]
        public async Task<string> cambiarClave([FromBody] UsuarioCambiarClaveRequest req)
        {
            return await Task.Run(() =>
            {
                bool ok;
                string msg = _usuario.cambiarClave(req.IdUsuario, req.Clave, out ok);
                return msg;
            });
        }


    }
}
