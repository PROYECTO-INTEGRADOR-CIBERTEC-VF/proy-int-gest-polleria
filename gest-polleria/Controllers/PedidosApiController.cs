using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosApiController : ControllerBase
    {
        PedidoDAO _pedido = new PedidoDAO();    
        // POST: api/PedidosApi/abrirmesa?idMesa=2&idTipoPedido=1&idMesero=3
        [HttpPost("abrirmesa")]
        public async Task<string> abrirMesa(
            [FromQuery] int idMesa,
            [FromQuery] int idTipoPedido,
            [FromQuery] int? idMesero = null)
        {
            return await Task.Run(() =>
            {
                int idPedidoNuevo;
                string msg = _pedido.abrirMesa(idMesa, idTipoPedido, idMesero, out idPedidoNuevo);
                return $"{msg} (IdPedidoNuevo={idPedidoNuevo})";
            });
        }

        PedidoDetalleDAO _detalle = new PedidoDetalleDAO();

        // POST: api/PedidosApi/agregardetalle?idPedido=1&idProducto=2&cantidad=1&observacion=Sin%20cebolla
        [HttpPost("agregardetalle")]
        public async Task<string> agregarDetalle(
            [FromQuery] int idPedido,
            [FromQuery] int idProducto,
            [FromQuery] decimal cantidad,
            [FromQuery] string? observacion = null)
        {
            return await Task.Run(() =>
            {
                int idDetNuevo;
                string msg = _detalle.insertarDetalle(idPedido, idProducto, cantidad, observacion, out idDetNuevo);
                return $"{msg} (IdPedidoDetalleNuevo={idDetNuevo})";
            });
        }

        // PUT: api/PedidosApi/actualizardetalle
        [HttpPut("actualizardetalle")]
        public async Task<string> actualizarDetalle(
            [FromQuery] int idPedidoDetalle,
            [FromQuery] decimal cantidad,
            [FromQuery] string? observacion = null)
        {
            return await Task.Run(() =>
                _detalle.actualizarDetalle(idPedidoDetalle, cantidad, observacion)
            );
        }

        // DELETE: api/PedidosApi/eliminardetalle/5
        [HttpDelete("eliminardetalle/{id}")]
        public async Task<string> eliminarDetalle(int id)
        {
            return await Task.Run(() =>
                _detalle.eliminarDetalle(id)
            );
        }

        // PUT: api/PedidosApi/enviaracocina/5
        [HttpPut("enviaracocina/{id}")]
        public async Task<string> enviarACocina(int id)
        {
            return await Task.Run(() =>
                _pedido.enviarACocina(id)
            );
        }

        // PUT: api/PedidosApi/cerrarmesa/5
        [HttpPut("cerrarmesa/{id}")]
        public async Task<string> cerrarMesa(int id)
        {
            return await Task.Run(() =>
                _pedido.cerrarMesa(id)
            );
        }

        [HttpPut("estado")]
        public async Task<string> cambiarEstado(int idPedido, int idEstado)
        {
            return await Task.Run(() =>
                _pedido.cambiarEstado(idPedido, idEstado)
            );
        }

        [HttpPost("crear")]
        public async Task<string> crearPedido([FromBody] Pedido p)
        {
            return await Task.Run(() =>
            {
                int idPedido;
                string msg = _pedido.registrarPedido(
                    p.IdTipoPedido,
                    p.IdCliente,
                    p.DireccionDelivery,
                    out idPedido
                );

                return $"{msg} (IdPedido={idPedido})";
            });
        }
    }
}
