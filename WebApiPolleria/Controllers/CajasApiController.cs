using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CajasApiController : ControllerBase
    {
        CajaDAO _caja = new CajaDAO();

        // POST: api/CajasApi/abrir
        [HttpPost("abrir")]
        public async Task<string> abrirCaja([FromBody] Caja c)
        {
            return await Task.Run(() =>
            {
                int idCaja;
                string msg = _caja.abrirCaja(c, out idCaja);
                return $"{msg} (IdCajaTurno={idCaja})";
            });
        }

        [HttpPut("cerrar")]
        public async Task<string> cerrarCaja([FromBody] Caja c)
        {
            return await Task.Run(() =>
                _caja.cerrarCaja(c)
            );
        }

    }
}
