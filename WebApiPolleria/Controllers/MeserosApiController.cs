using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeserosApiController : ControllerBase
    {
        MeseroDAO _mesero = new MeseroDAO();

        // 1. LISTAR CON FILTROS (Nombre/DNI y Turno)
        [HttpGet("listar")]
        public IActionResult listar(string nombre = "", string turno = "")
        {
            var lista = _mesero.listar(nombre, turno);
            return Ok(lista);
        }

        // 2. BUSCAR POR ID
        [HttpGet("buscar/{id}")]
        public IActionResult buscar(int id)
        {
            var obj = _mesero.buscar(id);
            if (obj == null)
            {
                return NotFound("Mesero no encontrado");
            }
            return Ok(obj);
        }

        // 3. REGISTRAR NUEVO MESERO
        [HttpPost("registrar")]
        public IActionResult registrar([FromBody] Mesero m)
        {
            if (m == null) return BadRequest("Datos inválidos");

            bool ok = _mesero.registrar(m);
            if (ok)
            {
                return Ok("Mesero registrado correctamente");
            }
            else
            {
                return BadRequest("Error al registrar el mesero en la base de datos");
            }
        }

        // 4. ACTUALIZAR MESERO (CON VALIDACIÓN)
        [HttpPut("actualizar")]
        public async Task<IActionResult> actualizar([FromBody] Mesero m)
        {
            if (m == null) return BadRequest("Error: Objeto nulo");

            string mensaje = await Task.Run(() => _mesero.actualizarConMensaje(m));

            if (mensaje.Contains("No se puede"))
            {
                return BadRequest(mensaje);
            }

            return Ok(mensaje);
        }

        // 5. CAMBIAR ESTADO (ACTIVAR/DESACTIVAR)
        [HttpPut("cambiarestado/{id}")]
        public IActionResult cambiarEstado(int id)
        {
            // Llamamos al método unificado del DAO que usa ExecuteScalar
            string mensaje = _mesero.cambiarEstadoUnificado(id);

            if (mensaje.StartsWith("Error"))
            {
                return BadRequest(mensaje);
            }

            return Ok(mensaje);
        }

        // 6. ASIGNAR MESA A MESERO
        [HttpPost("asignarmesa/{idMesero}/{idMesa}")]
        public IActionResult asignarMesa(int idMesero, int idMesa)
        {
            string mensaje = _mesero.asignarMesa(idMesero, idMesa);

            if (mensaje.Contains("Aviso") || mensaje.Contains("Error"))
            {
                return BadRequest(mensaje);
            }

            return Ok(mensaje);
        }

        // 7. LISTAR SOLO MESAS DISPONIBLES
        [HttpGet("mesasDisponibles")]
        public IActionResult listarMesasDisponibles()
        {
            var lista = _mesero.listarMesasDisponibles();
            return Ok(lista);
        }
    }
}