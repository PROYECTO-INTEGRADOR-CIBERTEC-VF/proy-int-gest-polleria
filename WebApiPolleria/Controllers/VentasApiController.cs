using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;
using WebApiPolleria.Services;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasApiController : ControllerBase
    {
        VentaDAO _venta = new VentaDAO();

        [HttpPost("registrar")]
        public async Task<string> registrarVenta([FromBody] Venta v)
        {
            return await Task.Run(() =>
            {
                int idVenta;
                string msg = _venta.registrarVenta(v, out idVenta);
                return $"{msg} (IdVentaNueva={idVenta})";
            });
        }

        [HttpPut("anular")]
        public async Task<string> anularVenta([FromBody] AnulacionVenta a)
        {
            return await Task.Run(() =>
                _venta.anularVenta(a)
            );
        }

        [HttpGet("consultar")]
        public async Task<IEnumerable<VentaConsultaDTO>> consultar(
        [FromQuery] string? serie = null,
        [FromQuery] string? numero = null,
        [FromQuery] DateTime? fechaInicio = null,
        [FromQuery] DateTime? fechaFin = null,
        [FromQuery] int? idEstadoComprobante = null,
        [FromQuery] int? idCliente = null)
        {
            return await Task.Run(() =>
                _venta.consultar(serie, numero, fechaInicio, fechaFin, idEstadoComprobante, idCliente)
            );
        }


        private readonly VentaService _ventaService = new VentaService();

        [HttpPost("generar-electronico")]
        public async Task<string> generarElectronico(
            [FromBody] GeneracionElectronica g)
        {
            return await Task.Run(() =>
                _ventaService.GenerarElectronicoConPdf(g)
            );
        }

        [HttpPost("enviar-sunat/{idVenta}")]
        public async Task<string> enviarSunat(int idVenta)
        {
            return await Task.Run(() =>
                _ventaService.EnviarSunatSimulado(idVenta)
            );
        }

        [HttpPost("recibir-cdr/{idVenta}")]
        public async Task<string> recibirCdr(int idVenta)
        {
            return await Task.Run(() =>
                _ventaService.RecibirCdrSimulado(idVenta)
            );
        }

        [HttpPost("enviar-cliente/{idVenta}")]
        public async Task<object> enviarCliente(int idVenta)
        {
            return await Task.Run(() =>
                _ventaService.EnviarCliente(idVenta)
            );
        }



    }
}
