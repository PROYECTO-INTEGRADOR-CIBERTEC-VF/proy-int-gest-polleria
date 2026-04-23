using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosApiController : ControllerBase
    {
        VentaPagoDAO _pago = new VentaPagoDAO();

        // POST: api/PagosApi/registrar
        [HttpPost("registrar")]
        public async Task<string> registrarPago([FromBody] VentaPago p)
        {
            return await Task.Run(() =>
                _pago.registrarPago(p)
            );
        }
    }
}
