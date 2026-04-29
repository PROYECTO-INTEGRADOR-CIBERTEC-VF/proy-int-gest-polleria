using Microsoft.AspNetCore.Mvc;
using gest_polleria.DAO;
using gest_polleria.Models;

namespace gest_polleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoApiController : ControllerBase
    {
        private readonly PedidoDAO _pedido = new PedidoDAO();
        private readonly PedidoDetalleDAO _detalle = new PedidoDetalleDAO();

        [HttpGet("abiertos")]
        public async Task<IEnumerable<PedidoAbierto>> listarAbiertos()
        {
            return await Task.Run(() => _pedido.listarPedidosAbiertos());
        }

        [HttpGet("listar")]
        public async Task<IEnumerable<PedidoOperativo>> listar()
        {
            return await Task.Run(() => _pedido.listarPedidosOperativos());
        }

        [HttpGet("carta")]
        public async Task<IEnumerable<ProductoCarta>> carta()
        {
            return await Task.Run(() => _pedido.listarCarta());
        }

        [HttpGet("detalle/{idPedido}")]
        public async Task<IEnumerable<PedidoDetalleLinea>> detalle(int idPedido)
        {
            return await Task.Run(() => _detalle.listarDetallePedido(idPedido));
        }

        [HttpPost("abrirmesa")]
        public async Task<string> abrirMesa(
            [FromQuery] int idMesa,
            [FromQuery] int idTipoPedido,
            [FromQuery] int? idMesero = null)
        {
            return await Task.Run(() =>
            {
                int idPedidoNuevo;
                return _pedido.abrirMesa(idMesa, idTipoPedido, idMesero, out idPedidoNuevo);
            });
        }

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
                return _detalle.insertarDetalle(idPedido, idProducto, cantidad, observacion, out idDetNuevo);
            });
        }

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

        [HttpDelete("eliminardetalle/{id}")]
        public async Task<string> eliminarDetalle(int id)
        {
            return await Task.Run(() =>
                _detalle.eliminarDetalle(id)
            );
        }

        [HttpPut("enviaracocina/{id}")]
        public async Task<string> enviarACocina(int id)
        {
            return await Task.Run(() =>
                _pedido.enviarACocina(id)
            );
        }

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
                return _pedido.registrarPedido(
                    p.IdTipoPedido,
                    p.IdCliente,
                    p.DireccionDelivery,
                    out idPedido
                );
            });
        }
    }
}
