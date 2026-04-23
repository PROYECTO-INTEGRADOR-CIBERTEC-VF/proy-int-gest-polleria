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

        [HttpGet]
        public IActionResult Get()
        {

            var lista = _dao.ListarMeseros();
            if (lista.Count == 0) return NotFound("No hay meseros registrados.");
            return Ok(lista);
        }

        [HttpPost("registrar-meserobd")]
        public IActionResult RegistrarMesero(Mesero reg)
        {
            // Llama al método RegistrarMesero del DAO
            string mensaje = _dao.RegistrarMesero(reg);
            return Ok(mensaje);
        }

        // POST: asignar-mesero-mesa
        [HttpPost("asignar-mesero-mesa")]
        public IActionResult AsignarMeseroMesa(int idMesero, int idMesa)
        {
            // Llama al método AsignarMeseroMesa del DAO
            string mensaje = _dao.AsignarMeseroMesa(idMesero, idMesa);
            return Ok(mensaje);
        }
    }
}