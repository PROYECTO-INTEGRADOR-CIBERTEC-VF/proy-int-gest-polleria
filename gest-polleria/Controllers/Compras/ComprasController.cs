using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using gest_polleria.Models;

namespace gest_polleria.Controllers.Compras
{
    public class ComprasController : Controller
    {
        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7173/")
        };

        public IActionResult Compra()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Compra c)
        {
            var json = JsonSerializer.Serialize(c);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("api/ComprasApi/registrar", content);

            TempData["Mensaje"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction("Compra");
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(CompraDetalle d)
        {
            var json = JsonSerializer.Serialize(d);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(
                "api/ComprasDetalleApi/agregar",
                content
            );

            TempData["Mensaje"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction("Compra");
        }
    }
}

