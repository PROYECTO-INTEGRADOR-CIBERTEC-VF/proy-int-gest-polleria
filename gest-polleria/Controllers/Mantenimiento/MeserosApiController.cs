using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Json; // Asegúrate de tener esta referencia para ReadFromJsonAsync

namespace gest_polleria.Controllers
{
    public class MeserosApiController : Controller
    {
        private readonly string _baseUrl = "https://localhost:7221/api/MeserosApi/";

        // 1. LISTADO Y BÚSQUEDA
        public async Task<IActionResult> Index(string filtro = "", string turno = "")
        {
            List<Mesero> lista = new List<Mesero>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                string url = $"listar?nombre={Uri.EscapeDataString(filtro ?? "")}&turno={Uri.EscapeDataString(turno ?? "")}";

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<Mesero>>(json) ?? new List<Mesero>();
                }
                else
                {
                    ViewBag.Error = "No se pudo conectar con el servicio de datos.";
                }
            }
            ViewBag.FiltroActual = filtro;
            ViewBag.TurnoActual = turno;
            return View(lista);
        }

        // 2. REGISTRAR
        public IActionResult RegistrarMesero() => View(new Mesero { Activo = true });

        [HttpPost]
        public async Task<IActionResult> RegistrarMesero(Mesero reg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                string json = JsonConvert.SerializeObject(reg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("registrar", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = "Mesero registrado correctamente.";
                    return RedirectToAction("Index");
                }
                string respuesta = await response.Content.ReadAsStringAsync();
                ViewBag.Mensaje = "Error: " + respuesta;
            }
            return View(reg);
        }

        // 3. EDICIÓN (GET): Carga los datos en el formulario
        [HttpGet]
        public async Task<IActionResult> EditarMesero(int id)
        {
            Mesero? obj = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);

                HttpResponseMessage response = await client.GetAsync($"buscar/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string js = await response.Content.ReadAsStringAsync();
                    obj = JsonConvert.DeserializeObject<Mesero>(js);
                }
            }

            if (obj == null)
            {
                TempData["MensajeError"] = "No se encontró el mesero solicitado.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // 3. EDICIÓN (POST): Guarda los cambios
        [HttpPost]
        public async Task<IActionResult> EditarMesero(Mesero obj)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);

                string json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("actualizar", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = errorMsg;
                    return View(obj);
                }
            }
        }

        // 4. CAMBIAR ESTADO 
        public async Task<IActionResult> CambiarEstado(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);

                HttpResponseMessage response = await client.PutAsync($"cambiarestado/{id}", null);

                string respuesta = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = respuesta;
                }
                else
                {
                    TempData["MensajeError"] = "Error al procesar: " + respuesta;
                }
            }
            return RedirectToAction("Index");
        }


        // 5. ASIGNAR MESERO A MESA (VISTA)
        [HttpGet]
        public async Task<IActionResult> AsignarMeseroMesa(int? idMesero)
        {
            if (idMesero != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);

                    HttpResponseMessage respMesero = await client.GetAsync($"buscar/{idMesero}");
                    if (respMesero.IsSuccessStatusCode)
                    {
                        string js = await respMesero.Content.ReadAsStringAsync();
                        var mesero = JsonConvert.DeserializeObject<Mesero>(js);
                        if (mesero != null)
                        {
                            ViewBag.NombreMesero = $"{mesero.Nombre} {mesero.Apellido}";
                            ViewBag.IdMeseroSeleccionado = mesero.IdMesero;
                        }
                    }

                    HttpResponseMessage respMesas = await client.GetAsync("mesasDisponibles");
                    if (respMesas.IsSuccessStatusCode)
                    {
                        string jsMesas = await respMesas.Content.ReadAsStringAsync();
                        var listaMesas = JsonConvert.DeserializeObject<List<Mesa>>(jsMesas);
                        ViewBag.ListaMesas = listaMesas;
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AsignarMeseroMesa(int idMesero, int idMesa)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);

                HttpResponseMessage response = await client.PostAsync($"asignarmesa/{idMesero}/{idMesa}", null);
                string mensaje = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = mensaje;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MensajeError"] = mensaje;

                    ViewBag.IdMeseroSeleccionado = idMesero;
                    return View();
                }
            }
        }
    }
}