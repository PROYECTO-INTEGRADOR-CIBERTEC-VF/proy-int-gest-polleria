using appPolleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppPolleria.Controllers
{
    public class MesaController : Controller
    {
        private readonly HttpClient _http;

        public MesaController()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri("https://localhost:7173/api/");
            // ⬆️ AJUSTA el puerto si es distinto
        }

        // =======================
        // LISTAR
        // =======================
        public async Task<IActionResult> Mesa(bool soloActivas = true, string? buscar = null)
        {
            string url = $"MesasApi/listar?soloActivas={soloActivas}&buscar={buscar}";
            var json = await _http.GetStringAsync(url);

            var lista = JsonConvert.DeserializeObject<List<Mesa>>(json)
                        ?? new List<Mesa>();
            ViewBag.SoloActivas = soloActivas;
            ViewBag.Buscar = buscar;
            return View(lista);
        }

        // =======================
        // BUSCAR (EDITAR)
        // =======================
        [HttpGet]
        public async Task<IActionResult> Buscar(int id)
        {
            var json = await _http.GetStringAsync($"MesasApi/buscar/{id}");
            var mesa = JsonConvert.DeserializeObject<Mesa>(json);

            return Json(mesa);
        }

        // =======================
        // REGISTRAR
        // =======================
        [HttpPost]
        public async Task<IActionResult> Registrar(Mesa m)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(m),
                Encoding.UTF8,
                "application/json"
            );

            await _http.PostAsync("MesasApi/registrar", content);
            return RedirectToAction("Mesa");
        }
        [HttpGet]
        public IActionResult Registrar()
        {
            return View("Registrar");
        }


        // =======================
        // ACTUALIZAR
        // =======================
        [HttpPost]
        public async Task<IActionResult> Actualizar(Mesa m)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(m),
                Encoding.UTF8,
                "application/json"
            );

            await _http.PutAsync("MesasApi/actualizar", content);
            return RedirectToAction("Mesa");
        }
        [HttpGet]
        public async Task<IActionResult> Actualizar(int id)
        {
            var json = await _http.GetStringAsync($"MesasApi/buscar/{id}");
            var mesa = JsonConvert.DeserializeObject<Mesa>(json);

            if (mesa == null)
                return NotFound();

            return View("Actualizar", mesa);
        }


        // =======================
        // DESACTIVAR
        // =======================
        public async Task<IActionResult> Desactivar(int id)
        {
            await _http.DeleteAsync($"MesasApi/desactivar/{id}");
            return RedirectToAction("Mesa");
        }

        // =======================
        // ACTIVAR
        // =======================
        public async Task<IActionResult> Activar(int id)
        {
            await _http.PutAsync($"MesasApi/activar/{id}", null);
            return RedirectToAction("Mesa");
        }
    }
}
