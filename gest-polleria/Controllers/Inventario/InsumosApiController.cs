using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace gest_polleria.Controllers.Inventario
{
    public class InsumosApiController : Controller
    {
        // URLs base configuradas para tu entorno local
        private readonly string _apiBaseUrl = "https://localhost:7173/api/InsumosApi/";
        private readonly string _catBaseUrl = "https://localhost:7173/api/CatalogosApi/";

        // ==========================================================
        // 1. LISTADO DE INSUMOS (ALMACÉN)
        // ==========================================================
        public async Task<IActionResult> ListadoInsumos(bool soloActivos = true, string? buscar = null)
        {
            List<Insumo> lista = new List<Insumo>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                // GET: api/InsumosApi/listar?soloActivos=true&buscar=...
                HttpResponseMessage response = await client.GetAsync($"listar?soloActivos={soloActivos}&buscar={buscar}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<Insumo>>(json) ?? new List<Insumo>();
                }
            }
            return View(lista);
        }

        // ==========================================================
        // 2. REGISTRAR NUEVO INSUMO
        // ==========================================================
        public async Task<IActionResult> RegistrarInsumo()
        {
            // Consumo de Catálogos para obtener las Unidades de Medida (Kg, Unid, etc.)
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_catBaseUrl);
                var response = await client.GetAsync("unidades");
                if (response.IsSuccessStatusCode)
                {
                    var unitJson = await response.Content.ReadAsStringAsync();
                    ViewBag.Unidades = JsonConvert.DeserializeObject<List<UnidadMedida>>(unitJson);
                }
            }
            // El stock inicial siempre debe ser 0; se carga vía compras o ajustes
            return View(new Insumo { Activo = true, StockActual = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarInsumo(Insumo reg)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                string json = JsonConvert.SerializeObject(reg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // POST: api/InsumosApi/registrar
                HttpResponseMessage response = await client.PostAsync("registrar", content);
                respuesta = await response.Content.ReadAsStringAsync();
            }

            if (respuesta.Contains("correctamente"))
            {
                TempData["Mensaje"] = respuesta;
                return RedirectToAction("ListadoInsumos");
            }

            ViewBag.Mensaje = respuesta;
            return View(reg);
        }

        // ==========================================================
        // 3. EDICIÓN DE INSUMO
        // ==========================================================
        public async Task<IActionResult> EdicionInsumo(int id)
        {
            Insumo? reg = null;
            using (var client = new HttpClient())
            {
                // 1. Obtener datos actuales del insumo
                client.BaseAddress = new Uri(_apiBaseUrl);
                var respInsumo = await client.GetAsync($"buscar/{id}");
                reg = JsonConvert.DeserializeObject<Insumo>(await respInsumo.Content.ReadAsStringAsync());

                // 2. Obtener unidades para el desplegable (ComboBox)
                var clientCat = new HttpClient { BaseAddress = new Uri(_catBaseUrl) };
                var respCat = await clientCat.GetAsync("unidades");
                ViewBag.Unidades = JsonConvert.DeserializeObject<List<UnidadMedida>>(await respCat.Content.ReadAsStringAsync());
            }

            if (reg == null) return RedirectToAction("ListadoInsumos");
            return View(reg);
        }

        [HttpPost]
        public async Task<IActionResult> EdicionInsumo(Insumo reg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                string json = JsonConvert.SerializeObject(reg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // PUT: api/InsumosApi/actualizar
                await client.PutAsync("actualizar", content);
            }
            TempData["Mensaje"] = "Datos del insumo actualizados correctamente.";
            return RedirectToAction("ListadoInsumos");
        }

        // ==========================================================
        // 4. DESACTIVAR INSUMO (BORRADO LÓGICO)
        // ==========================================================
        public async Task<IActionResult> DesactivarInsumo(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                // DELETE: api/InsumosApi/desactivar/{id}
                HttpResponseMessage response = await client.DeleteAsync($"desactivar/{id}");
                TempData["Mensaje"] = await response.Content.ReadAsStringAsync();
            }
            return RedirectToAction("ListadoInsumos");
        }

        // Acción por defecto: Redirige al listado principal
        public IActionResult Index() => RedirectToAction("ListadoInsumos");
    }
}
