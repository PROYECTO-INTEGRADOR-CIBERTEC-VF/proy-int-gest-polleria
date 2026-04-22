using Microsoft.AspNetCore.Mvc;
using gest_polleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
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
    }
}