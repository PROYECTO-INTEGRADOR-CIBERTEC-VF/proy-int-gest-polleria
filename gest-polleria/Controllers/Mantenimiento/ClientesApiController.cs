using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace gest_polleria.Controllers.Mantenimiento
{
    public class ClientesApiController : Controller
    {
        private readonly string _baseUrl = "https://localhost:7173/api/ClientesApi/";

        // ==========================================================
        // 1. LISTADO DE CLIENTES (Con validación de respuesta)
        // ==========================================================
        public async Task<IActionResult> ListadoClientes(bool? esEmpresa = null, string? buscar = null)
        {
            List<Cliente> temporal = new List<Cliente>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                // GET: api/ClientesApi/listar?esEmpresa=...&buscar=...
                HttpResponseMessage response = await client.GetAsync($"listar?esEmpresa={esEmpresa}&buscar={buscar}");

                if (response.IsSuccessStatusCode)
                {
                    string mensaje = await response.Content.ReadAsStringAsync();
                    // Solo intentamos deserializar si el contenido parece un JSON (empieza con [)
                    if (!string.IsNullOrEmpty(mensaje) && mensaje.Trim().StartsWith("["))
                    {
                        temporal = JsonConvert.DeserializeObject<List<Cliente>>(mensaje) ?? new List<Cliente>();
                    }
                }
                else
                {
                    ViewBag.Error = "La API no respondió correctamente. Verifique si el servicio está activo.";
                }
            }
            return View(temporal);
        }

        // ==========================================================
        // 2. DETALLE DEL CLIENTE
        // ==========================================================
        public async Task<IActionResult> DetalleCliente(int id)
        {
            Cliente? reg = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                HttpResponseMessage response = await client.GetAsync($"buscar/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string mensaje = await response.Content.ReadAsStringAsync();
                    reg = JsonConvert.DeserializeObject<Cliente>(mensaje);
                }
            }
            if (reg == null) return RedirectToAction("ListadoClientes");
            return View(reg);
        }

        // ==========================================================
        // 3. REGISTRAR CLIENTE
        // ==========================================================
        public IActionResult RegistrarCliente() => View(new Cliente());

        [HttpPost]
        public async Task<IActionResult> RegistrarCliente(Cliente reg)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                string json = JsonConvert.SerializeObject(reg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("registrar", content);
                respuesta = await response.Content.ReadAsStringAsync();
            }

            // Si el registro fue exitoso, redirigimos al listado
            if (respuesta.ToLower().Contains("correctamente") || respuesta.ToLower().Contains("exito"))
            {
                TempData["Mensaje"] = "Cliente registrado con éxito.";
                return RedirectToAction("ListadoClientes");
            }

            ViewBag.Mensaje = respuesta;
            return View(reg);
        }
    }
}
