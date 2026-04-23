using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasApiController : ControllerBase
    {
        CompraDAO _compra = new CompraDAO();

        // RF-009 Registro de compras
        // POST: api/ComprasApi/registrar
        [HttpPost("registrar")]
        public async Task<string> registrar([FromBody] Compra c)
        {
            return await Task.Run(() =>
            {
                int idCompra;
                string msg = _compra.registrarCompra(c, out idCompra);
                return $"{msg} (IdCompra={idCompra})";
            });
        }
    }
}
