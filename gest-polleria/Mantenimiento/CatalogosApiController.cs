using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace appPolleria.Controllers.Mantenimiento
{
    public class CatalogosApiController : Controller
    {
        private readonly string _apiBaseUrl = "https://localhost:7173/api/CatalogosApi/";

        // ==========================================================
        // SECCIÓN: VISTAS DE LISTADO (CORREGIDAS)
        // ==========================================================

        public async Task<IActionResult> ListadoCategorias()
        {
            List<CategoriaProducto> lista = new List<CategoriaProducto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                HttpResponseMessage response = await client.GetAsync("categorias");
                if (response.IsSuccessStatusCode)
                {
                    string mensaje = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<CategoriaProducto>>(mensaje) ?? new List<CategoriaProducto>();
                }
            }
            return View(lista); 
        }

        public async Task<IActionResult> ListadoUnidades()
        {
            List<UnidadMedida> lista = new List<UnidadMedida>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                HttpResponseMessage response = await client.GetAsync("unidades");
                if (response.IsSuccessStatusCode)
                {
                    string mensaje = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<UnidadMedida>>(mensaje) ?? new List<UnidadMedida>();
                }
            }
            return View(lista);
        }


        public async Task<IActionResult> ListadoRoles()
        {
            List<Rol> lista = new List<Rol>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                HttpResponseMessage response = await client.GetAsync("roles");
                if (response.IsSuccessStatusCode)
                {
                    string mensaje = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<Rol>>(mensaje) ?? new List<Rol>();
                }
            }
            return View(lista);
        }

        // ==========================================================
        // SECCIÓN: REGISTROS (POST)
        // ==========================================================

        public IActionResult RegistrarCategoria() => View(new CategoriaProducto());

        [HttpPost]
        public async Task<IActionResult> RegistrarCategoria(CategoriaProducto reg)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                string json = JsonConvert.SerializeObject(reg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("registrar-categoria", content);
                respuesta = await response.Content.ReadAsStringAsync();
            }

            if (respuesta.ToLower().Contains("éxito") || respuesta.ToLower().Contains("correctamente"))
            {
                TempData["Mensaje"] = "Categoría guardada con éxito";
                return RedirectToAction("ListadoCategorias");
            }

            ViewBag.Mensaje = respuesta;
            return View(reg);
        }

        public IActionResult Index() => RedirectToAction("ListadoCategorias");
    }
}