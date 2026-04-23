using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebApiPolleria.Controllers;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace gest_polleria.Controllers
{
    public class PedidosController : Controller
    {
        private readonly HttpClient _http;

        public PedidosController()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri("https://localhost:7173/api/");
        }

        // ===============================
        // ABRIR MESA
        // ===============================
        // GET: Pedidos/AbrirMesa
        [HttpGet]
        public IActionResult Pedido(int idPedido)
        {
            ViewBag.IdPedido = idPedido;
            ViewBag.Detalles = new List<object>();
            return View();
        }


        [HttpGet]
        public IActionResult AbrirMesa()
        {
            return View(new AbrirMesaViewModel());
        }

        // POST: Pedidos/AbrirMesa
        [HttpPost]
        public IActionResult AbrirMesa(int idMesa, int idTipoPedido, int? idMesero)
        {
            PedidoDAO _pedido = new PedidoDAO();

            // Llamar al DAO para abrir la mesa
            int idPedidoNuevo;
            string msg = _pedido.abrirMesa(idMesa, idTipoPedido, idMesero, out idPedidoNuevo);

            if (idPedidoNuevo > 0)
            {
                // Redirige a la vista del pedido abierto mostrando los detalles
                return RedirectToAction("Pedido", new { idPedido = idPedidoNuevo });
            }

            // Si hubo algún error, se puede mostrar mensaje y volver al formulario
            ViewBag.Error = msg;
            return View();
        }

        // ===============================
        // CREAR PEDIDO (DELIVERY / CLIENTE)
        // ===============================
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Pedidos p)
        {
            PedidoDAO _pedido = new PedidoDAO();

            int idPedidoNuevo;
            string msg = _pedido.registrarPedido(p.IdTipoPedido, p.IdCliente, p.DireccionDelivery, out idPedidoNuevo);

            if (idPedidoNuevo > 0)
            {
                // Redirige a la vista del pedido recién registrado
                return RedirectToAction("Pedido", new { idPedido = idPedidoNuevo });
            }

            // Si hubo algún error
            ViewBag.Error = msg;
            return View(p);
        }

        // ===============================
        // AGREGAR DETALLE
        // ===============================
        [HttpGet]
        public IActionResult AgregarDetalle(int idPedido)
        {
            ViewBag.IdPedido = idPedido;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarDetalle(
    int idPedido,
    int idProducto,
    decimal cantidad,
    string? observacion)
        {
            // LLAMADA AL WEB API
            var response = await _http.PostAsync(
                $"PedidosApi/agregardetalle" +
                $"?idPedido={idPedido}" +
                $"&idProducto={idProducto}" +
                $"&cantidad={cantidad}" +
                $"&observacion={observacion}",
                null
            );

            // (opcional) leer mensaje
            var msg = await response.Content.ReadAsStringAsync();

            // REGRESAR AL PEDIDO
            return RedirectToAction("Pedido", new { idPedido });
        }


        // ===============================
        // ACTUALIZAR DETALLE
        // ===============================
        // GET: Pedidos/ActualizarDetalle/5
        public IActionResult ActualizarDetalle(int idPedidoDetalle, int idPedido)
        {
            PedidoDetalleDAO dao = new PedidoDetalleDAO();
            var detalle = dao.buscarDetalle(idPedidoDetalle);

            if (detalle == null)
            {
                TempData["Error"] = "Detalle no encontrado";
                return RedirectToAction("Pedido", new { idPedido = idPedido });
            }

            ViewBag.IdPedido = idPedido;
            return View(detalle);
        }

        [HttpPost]
        public IActionResult ActualizarDetalle(int idPedidoDetalle, decimal cantidad, string observacion)
        {
            PedidoDetalleDAO dao = new PedidoDetalleDAO();
            string msg = dao.actualizarDetalle(idPedidoDetalle, cantidad, observacion);

            TempData["Mensaje"] = msg;
            return RedirectToAction("Pedido"); // Opcional: podrías enviar el idPedido si quieres mostrar la lista actualizada
        }


        // ===============================
        // ELIMINAR DETALLE
        // ===============================
        [HttpGet]
        public IActionResult EliminarDetalle()
        {
            return View();
        }


        // ===============================
        // ENVIAR A COCINA
        // ===============================
        [HttpGet]
        public IActionResult EnviarACocina()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CerrarMesa()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CambiarEstado()
        {
            return View();
        }

        // ===============================
        // CAMBIAR ESTADO
        // ===============================
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int idPedido, int idEstado)
        {
            string url = $"PedidosApi/estado?idPedido={idPedido}&idEstado={idEstado}";
            await _http.PutAsync(url, null);

            return RedirectToAction("Pedido");
        }

        // ===============================
        // INDEX (placeholder)
        // ===============================
        public IActionResult Index()
        {
            return View();
        }
    }
}
