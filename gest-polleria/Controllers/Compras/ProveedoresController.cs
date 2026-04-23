using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using gest_polleria.Models;

namespace gest_polleria.Controllers.Compras
{
    public class ProveedoresController : Controller
    {
        public class ProveedorController : Controller
        {
            private readonly HttpClient _http;

            public ProveedorController()
            {
                _http = new HttpClient();
                _http.BaseAddress = new Uri("https://localhost:7173/api/");
                // ⚠️ ajusta el puerto si tu API usa otro
            }

            // =========================
            // LISTAR + BUSCAR
            // =========================
            public async Task<IActionResult> Proveedor(bool soloActivos = true, string? buscar = null)
            {
                string url = $"ProveedoresApi/listar?soloActivos={soloActivos}&buscar={buscar}";
                var json = await _http.GetStringAsync(url);

                var lista = JsonConvert.DeserializeObject<List<Proveedor>>(json)
                            ?? new List<Proveedor>();

                ViewBag.SoloActivos = soloActivos;
                ViewBag.Buscar = buscar;

                return View("Proveedor", lista);
            }

            // =========================
            // BUSCAR (EDITAR)
            // =========================
            [HttpGet]
            public async Task<IActionResult> Buscar(int id)
            {
                var json = await _http.GetStringAsync($"ProveedoresApi/buscar/{id}");
                var proveedor = JsonConvert.DeserializeObject<Proveedor>(json);

                return Json(proveedor);
            }

            // =========================
            // REGISTRAR (GET)
            // =========================
            [HttpGet]
            public IActionResult Registrar()
            {
                return View("Registrar");
            }

            // =========================
            // REGISTRAR (POST)
            // =========================
            [HttpPost]
            public async Task<IActionResult> Registrar(Proveedor p)
            {
                var content = new StringContent(
                    JsonConvert.SerializeObject(p),
                    Encoding.UTF8,
                    "application/json"
                );

                await _http.PostAsync("ProveedoresApi/registrar", content);
                return RedirectToAction("Proveedor");
            }

            // =========================
            // ACTUALIZAR (GET)
            // =========================
            [HttpGet]
            public async Task<IActionResult> Actualizar(int id)
            {
                var json = await _http.GetStringAsync($"ProveedoresApi/buscar/{id}");
                var proveedor = JsonConvert.DeserializeObject<Proveedor>(json);

                if (proveedor == null)
                    return NotFound();

                return View("Actualizar", proveedor);
            }

            // =========================
            // ACTUALIZAR (POST)
            // =========================
            [HttpPost]
            public async Task<IActionResult> Actualizar(Proveedor p)
            {
                var content = new StringContent(
                    JsonConvert.SerializeObject(p),
                    Encoding.UTF8,
                    "application/json"
                );

                await _http.PutAsync("ProveedoresApi/actualizar", content);
                return RedirectToAction("Proveedor");
            }

            // =========================
            // DESACTIVAR
            // =========================
            public async Task<IActionResult> Desactivar(int id)
            {
                await _http.DeleteAsync($"ProveedoresApi/desactivar/{id}");
                return RedirectToAction("Proveedor");
            }

            // =========================
            // ACTIVAR
            // =========================
            public async Task<IActionResult> Activar(int id)
            {
                await _http.PutAsync($"ProveedoresApi/activar/{id}", null);
                return RedirectToAction("Proveedor");
            }
        }
    }
}
