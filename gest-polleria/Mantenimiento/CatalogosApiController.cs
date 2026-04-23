using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace appPolleria.Controllers.Mantenimiento
{
    public class CatalogosApiController : Controller
    {
        private readonly string _apiBaseUrl = "https://localhost:7173/api/CatalogosApi/";

        // SECCIÓN: VISTAS DE LISTADO (CORREGIDAS)

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

        public async Task<IActionResult> ListadoMeseros()
        {
            List<Mesero> lista = new List<Mesero>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                HttpResponseMessage response = await client.GetAsync("meseros");
                if (response.IsSuccessStatusCode)
                {
                    string mensaje = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<Mesero>>(mensaje) ?? new List<Mesero>();
                }
            }
            // CAMBIO: Apunta a la nueva carpeta MeserosApi
            return View("~/Views/MeserosApi/ListadoMeseros.cshtml", lista);
        }

        // SECCIÓN: REGISTROS (POST)

        public IActionResult RegistrarCategoria() => View(new CategoriaProducto());

        [HttpPost]
        public async Task<IActionResult> RegistrarCategoria(CategoriaProducto reg)
        {
            return await EnviarPostApi(reg, "registrar-categoria", "ListadoCategorias");
        }

        public IActionResult RegistrarMesero()
        {
            // CAMBIO: Apunta a la nueva carpeta MeserosApi
            return View("~/Views/MeserosApi/RegistrarMesero.cshtml", new Mesero());
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarMesero(Mesero reg)
        {
            return await EnviarPostApi(reg, "registrar-meserobd", "ListadoMeseros");
        }

        // --- ASIGNACIÓN DE MESEROS A MESAS/ZONAS ---
        public IActionResult AsignarMeseroMesa()
        {
            // CAMBIO: Apunta a la nueva carpeta MeserosApi
            return View("~/Views/MeserosApi/AsignarMeseroMesa.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AsignarMeseroMesa(int idMesero, int idMesa)
        {
            var reg = new { idMesero = idMesero, idMesa = idMesa };
            return await EnviarPostApi(reg, "asignar-mesero-mesa", "ListadoMeseros");
        }

        // MÉTODO AUXILIAR PARA EVITAR REPETIR CÓDIGO
        private async Task<IActionResult> EnviarPostApi(object reg, string endpoint, string redirectAction)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                string json = JsonConvert.SerializeObject(reg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                respuesta = await response.Content.ReadAsStringAsync();
            }

            if (respuesta.ToLower().Contains("éxito") || respuesta.ToLower().Contains("correctamente"))
            {
                TempData["Mensaje"] = "Operación realizada con éxito";
                return RedirectToAction(redirectAction);
            }

            ViewBag.Mensaje = respuesta;

            // CAMBIO: Asegura que si falla el registro, regrese a la vista correcta en la carpeta MeserosApi
            if (redirectAction == "ListadoMeseros")
                return View("~/Views/MeserosApi/" + ControllerContext.ActionDescriptor.ActionName + ".cshtml", reg);

            return View(reg);
        }

        public IActionResult Index() => RedirectToAction("ListadoCategorias");
    }
}