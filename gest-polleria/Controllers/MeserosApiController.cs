using gest_polleria.DAO;
using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace gest_polleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeserosApiController : ControllerBase
    {
        private readonly MeseroDAO _mesero = new MeseroDAO();

        [HttpGet("listar")]
        public async Task<IEnumerable<Mesero>> listar()
        {
            return await Task.Run(() => _mesero.listar());
        }

        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] MeseroRegistroRequest request)
        {
            return await Task.Run(() =>
            {
                int idNuevo;
                return _mesero.registrar(
                    request.UserName,
                    request.ClaveHash,
                    request.NombreCompleto,
                    request.Email,
                    request.Telefono,
                    out idNuevo
                );
            });
        }

        [HttpPost("turnos/registrar")]
        public async Task<string> registrarTurno([FromBody] MeseroTurno turno)
        {
            return await Task.Run(() =>
            {
                int idTurno;
                return _mesero.registrarTurno(turno, out idTurno);
            });
        }

        [HttpPost("zonas/asignar")]
        public async Task<string> asignarZona([FromBody] AsignacionZonaRequest request)
        {
            return await Task.Run(() =>
            {
                int idAsignacion;
                return _mesero.asignarZona(request.IdUsuario, request.IdZona, out idAsignacion);
            });
        }

        [HttpGet("turnos")]
        public async Task<IEnumerable<MeseroTurno>> listarTurnos([FromQuery] int? idUsuario = null)
        {
            return await Task.Run(() => _mesero.listarTurnos(idUsuario));
        }

        [HttpGet("zonas")]
        public async Task<IEnumerable<MeseroZona>> listarZonas([FromQuery] int? idUsuario = null)
        {
            return await Task.Run(() => _mesero.listarZonasAsignadas(idUsuario));
        }
    }
}
