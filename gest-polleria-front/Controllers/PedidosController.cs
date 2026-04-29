using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using gest_polleria_front.Filters;
using gest_polleria_front.Models;
using gest_polleria_front.Services;

namespace gest_polleria_front.Controllers
{
    [SessionAuthorize]
    public class PedidosController : AppController
    {
        private readonly PolleriaApiService _api = new PolleriaApiService();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (IsMesero)
            {
                return RedirectToAction("Mesero");
            }

            return View(await BuildViewModelAsync(new AbrirMesaViewModel(), new PedidoDetalleFormViewModel(), new GestionPedidoViewModel()));
        }

        [HttpGet]
        [SessionAuthorize(RequiredRole = "MESERO")]
        public async Task<ActionResult> Mesero()
        {
            ViewBag.MeseroMode = true;
            return View("Index", await BuildViewModelAsync(new AbrirMesaViewModel(), new PedidoDetalleFormViewModel(), new GestionPedidoViewModel()));
        }

        [HttpGet]
        public async Task<JsonResult> DetallePedido(int idPedido)
        {
            try
            {
                var detalle = await _api.GetPedidoDetalleAsync(idPedido);
                return Json(new { ok = true, detalle }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { ok = false, message = "No pudimos cargar el detalle del pedido." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AbrirMesa(AbrirMesaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveForm = "abrirMesa";
                return await PedidoViewAsync(model, new PedidoDetalleFormViewModel(), new GestionPedidoViewModel());
            }

            try
            {
                await _api.AbrirMesaAsync(model);
                FlashSuccess("Mesa abierta correctamente.");
                return RedirectAfterPedidoMutation();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos abrir la mesa. Revisa los datos e inténtalo nuevamente.");
                ViewBag.ActiveForm = "abrirMesa";
                return await PedidoViewAsync(model, new PedidoDetalleFormViewModel(), new GestionPedidoViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarDetalle(PedidoDetalleFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveForm = "detallePedido";
                if (Request.IsAjaxRequest())
                {
                    return Json(new { ok = false, message = "Revisa los campos del detalle." });
                }

                return await PedidoViewAsync(new AbrirMesaViewModel(), model, new GestionPedidoViewModel());
            }

            try
            {
                var mensaje = model.IdPedidoDetalle > 0
                    ? "Detalle actualizado correctamente."
                    : "Producto agregado correctamente.";

                if (model.IdPedidoDetalle > 0)
                {
                    await _api.ActualizarDetalleAsync(model);
                }
                else
                {
                    await _api.AgregarDetalleAsync(model);
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(new { ok = true, message = mensaje });
                }

                FlashSuccess(mensaje);
                return RedirectAfterPedidoMutation();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos guardar el detalle. Inténtalo nuevamente.");
                ViewBag.ActiveForm = "detallePedido";

                if (Request.IsAjaxRequest())
                {
                    return Json(new { ok = false, message = "No pudimos guardar el detalle." });
                }

                return await PedidoViewAsync(new AbrirMesaViewModel(), model, new GestionPedidoViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EliminarDetalle(int idPedidoDetalle)
        {
            try
            {
                await _api.EliminarDetalleAsync(idPedidoDetalle);
                return Json(new { ok = true, message = "Detalle eliminado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { ok = false, message = "No pudimos eliminar el detalle." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnviarACocina(GestionPedidoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveForm = "estadoPedido";
                return await PedidoViewAsync(new AbrirMesaViewModel(), new PedidoDetalleFormViewModel(), model);
            }

            try
            {
                await _api.EnviarPedidoACocinaAsync(model.IdPedido);
                FlashSuccess("Pedido enviado a cocina correctamente.");
                return RedirectAfterPedidoMutation();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos enviar el pedido a cocina. Inténtalo nuevamente.");
                ViewBag.ActiveForm = "estadoPedido";
                return await PedidoViewAsync(new AbrirMesaViewModel(), new PedidoDetalleFormViewModel(), model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CerrarMesa(GestionPedidoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveForm = "estadoPedido";
                return await PedidoViewAsync(new AbrirMesaViewModel(), new PedidoDetalleFormViewModel(), model);
            }

            try
            {
                await _api.CerrarMesaAsync(model.IdPedido);
                FlashSuccess("Mesa cerrada correctamente.");
                return RedirectAfterPedidoMutation();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No pudimos cerrar la mesa. Inténtalo nuevamente.");
                ViewBag.ActiveForm = "estadoPedido";
                return await PedidoViewAsync(new AbrirMesaViewModel(), new PedidoDetalleFormViewModel(), model);
            }
        }

        private ActionResult RedirectAfterPedidoMutation()
        {
            return IsMesero ? RedirectToAction("Mesero") : RedirectToAction("Index");
        }

        private async Task<ActionResult> PedidoViewAsync(
            AbrirMesaViewModel apertura,
            PedidoDetalleFormViewModel detalle,
            GestionPedidoViewModel gestion)
        {
            if (IsMesero)
            {
                ViewBag.MeseroMode = true;
            }

            return View("Index", await BuildViewModelAsync(apertura, detalle, gestion));
        }

        private async Task<PedidosIndexViewModel> BuildViewModelAsync(
            AbrirMesaViewModel apertura,
            PedidoDetalleFormViewModel detalle,
            GestionPedidoViewModel gestion)
        {
            var model = new PedidosIndexViewModel
            {
                Apertura = apertura ?? new AbrirMesaViewModel(),
                Detalle = detalle ?? new PedidoDetalleFormViewModel(),
                Gestion = gestion ?? new GestionPedidoViewModel()
            };

            if (IsMesero && !model.Apertura.IdMesero.HasValue && CurrentUserId.HasValue)
            {
                model.Apertura.IdMesero = CurrentUserId.Value;
            }

            try
            {
                model.Mesas = await _api.GetMesasAsync(true);
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar las mesas. Inténtalo nuevamente.");
            }

            try
            {
                model.Meseros = await _api.GetMeserosAsync();
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar el personal. Inténtalo nuevamente.");
            }

            try
            {
                model.PedidosAbiertos = await _api.GetPedidosAbiertosAsync();
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar los pedidos abiertos. Inténtalo nuevamente.");
            }

            try
            {
                model.PedidosOperativos = await _api.GetPedidosOperativosAsync();
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar los pedidos. Inténtalo nuevamente.");
            }

            try
            {
                model.Carta = await _api.GetCartaAsync();
            }
            catch (Exception)
            {
                FlashError("No pudimos cargar los productos. Inténtalo nuevamente.");
            }

            return model;
        }
    }
}
