using Microsoft.AspNetCore.Mvc;
using gest_polleria.DAO;
using gest_polleria.Models;

namespace gest_polleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeserosApiController : ControllerBase
    {
        private readonly MeseroDAO _dao;

        public MeserosApiController(IConfiguration config)
        {
            _dao = new MeseroDAO(config);
        }

        // GET: api/MeserosApi/listar
        [HttpGet("listar")]
        public IActionResult Get()
        {
            var lista = _dao.ListarMeseros();
            if (lista.Count == 0) return NotFound("No hay meseros registrados.");
            return Ok(lista);
        }

        // NUEVO - GET: api/MeserosApi/buscar/5
        [HttpGet("buscar/{id}")]
        public IActionResult Buscar(int id)
        {
            var mesero = _dao.ListarMeseros().FirstOrDefault(m => m.IdMesero == id);
            if (mesero == null) return NotFound("Mesero no encontrado.");
            return Ok(mesero);
        }

        // POST: api/MeserosApi/registrar
        [HttpPost("registrar")]
        public IActionResult RegistrarMesero(Mesero reg)
        {
            if (reg == null) return BadRequest("Datos inválidos.");
            string mensaje = _dao.RegistrarMesero(reg);
            return Ok(new { mensaje });
        }

        // PUT: api/MeserosApi/actualizar
        [HttpPut("actualizar")]
        public IActionResult ActualizarMesero(Mesero reg)
        {
            if (reg == null) return BadRequest("Datos inválidos.");
            string mensaje = _dao.ActualizarMesero(reg);
            return Ok(new { mensaje });
        }

        // DELETE: api/MeserosApi/desactivar/5
        [HttpDelete("desactivar/{id}")]
        public IActionResult Desactivar(int id)
        {
            string mensaje = _dao.CambiarEstado(id, false);
            return Ok(new { mensaje });
        }

        // PUT: api/MeserosApi/activar/5
        [HttpPut("activar/{id}")]
        public IActionResult Activar(int id)
        {
            string mensaje = _dao.CambiarEstado(id, true);
            return Ok(new { mensaje });
        }

        // POST: api/MeserosApi/asignar-mesero-mesa
        [HttpPost("asignar-mesero-mesa")]
        public IActionResult AsignarMeseroMesa(int idMesero, int idMesa)
        {
            if (idMesero <= 0 || idMesa <= 0) return BadRequest("IDs inválidos.");
            string mensaje = _dao.AsignarMeseroMesa(idMesero, idMesa);
            return Ok(new { mensaje });
        }
    }
}