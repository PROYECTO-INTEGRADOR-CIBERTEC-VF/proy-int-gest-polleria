using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace gest_polleria.Controllers.Inventario
{
    public class AjustesInventarioApiController : Controller
    {
        // URLs de los Endpoints de la Web API (Puerto 7041)
        private readonly string _apiBaseUrl = "https://localhost:7173/api/AjustesInventarioApi/";
        private readonly string _insumoBaseUrl = "https://localhost:7173/api/InsumosApi/";

        // ==========================================================
        // 1. LISTADO HISTÓRICO DE AJUSTES
        // ==========================================================
        public async Task<IActionResult> ListadoAjustes()
        {
            List<AjusteInventario> lista = new List<AjusteInventario>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                HttpResponseMessage response = await client.GetAsync("listar");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<AjusteInventario>>(json) ?? new List<AjusteInventario>();
                }
            }
            return View(lista);
        }

        // ==========================================================
        // 2. REGISTRAR NUEVO AJUSTE (Mermas / Ingresos Extra)
        // ==========================================================
        public async Task<IActionResult> RegistrarAjuste(int? idInsumo = null)
        {
            // Cargamos solo los insumos para el desplegable
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_insumoBaseUrl);
                var response = await client.GetAsync("listar?soloActivos=true");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    ViewBag.Insumos = JsonConvert.DeserializeObject<List<Insumo>>(json);
                }
            }

            // Creamos el modelo solo con el ID del insumo si se proporciona
            var modelo = new AjusteInventario
            {
                IdInsumo = idInsumo ?? 0
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAjuste(AjusteInventario reg)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                string json = JsonConvert.SerializeObject(reg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("registrar", content);
                respuesta = await response.Content.ReadAsStringAsync();
            }

            if (respuesta.Contains("correctamente") || respuesta.Contains("exitoso"))
            {
                TempData["MensajeExito"] = "Ajuste procesado con éxito.";
                return RedirectToAction("ListadoInsumos", "InsumosApi");
            }

            ViewBag.Error = respuesta;
            // Si hay error, recargamos la lista de insumos para que el combo no salga vacío
            return await RegistrarAjuste(reg.IdInsumo);
        }

        public IActionResult Index() => RedirectToAction("ListadoAjustes");
    }
}

