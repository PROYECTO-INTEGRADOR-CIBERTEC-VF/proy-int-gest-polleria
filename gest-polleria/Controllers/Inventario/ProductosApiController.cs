using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace gest_polleria.Controllers.Inventario
{
    public class ProductosApiController : Controller
    {
            // URLs base de la Web API (Puerto 7041)
            private readonly string _apiBaseUrl = "https://localhost:7173/api/ProductosApi/";
            private readonly string _catBaseUrl = "https://localhost:7173/api/CatalogosApi/";

            // ==========================================================
            // 1. LISTADO DE PRODUCTOS (CARTA)
            // ==========================================================
            public async Task<IActionResult> ListadoProductosCarta(int? idCategoria = null, string? buscar = null)
            {
                List<Producto> lista = new List<Producto>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    // GET: api/ProductosApi/listar?idCategoria=...&buscar=...
                    HttpResponseMessage response = await client.GetAsync($"listar?idCategoria={idCategoria}&buscar={buscar}");

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        lista = JsonConvert.DeserializeObject<List<Producto>>(json) ?? new List<Producto>();
                    }
                }
                return View(lista);
            }

            // ==========================================================
            // 2. DETALLE DEL PRODUCTO
            // ==========================================================
            public async Task<IActionResult> DetalleProductoCarta(int id)
            {
                Producto? reg = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    HttpResponseMessage response = await client.GetAsync($"buscar/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        reg = JsonConvert.DeserializeObject<Producto>(json);
                    }
                }
                if (reg == null) return RedirectToAction("ListadoProductosCarta");
                return View(reg);
            }

            // ==========================================================
            // 3. REGISTRAR NUEVO PRODUCTO
            // ==========================================================
            public async Task<IActionResult> RegistrarProductoCarta()
            {
                // Cargamos categorías desde CatalogosApi para el ComboBox de la vista
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_catBaseUrl);
                    var response = await client.GetAsync("categorias");
                    if (response.IsSuccessStatusCode)
                    {
                        var catJson = await response.Content.ReadAsStringAsync();
                        ViewBag.Categorias = JsonConvert.DeserializeObject<List<CategoriaProducto>>(catJson);
                    }
                }
                return View(new Producto { Activo = true });
            }

            [HttpPost]
            public async Task<IActionResult> RegistrarProductoCarta(Producto reg)
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

                if (respuesta.Contains("correctamente"))
                {
                    TempData["Mensaje"] = respuesta;
                    return RedirectToAction("ListadoProductosCarta");
                }

                ViewBag.Mensaje = respuesta;
                return View(reg);
            }

            // ==========================================================
            // 4. EDICIÓN DE PRODUCTO
            // ==========================================================
            public async Task<IActionResult> EdicionProductoCarta(int id)
            {
                Producto? reg = null;
                using (var client = new HttpClient())
                {
                    // Obtener datos del producto
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var respProd = await client.GetAsync($"buscar/{id}");
                    reg = JsonConvert.DeserializeObject<Producto>(await respProd.Content.ReadAsStringAsync());

                    // Obtener categorías para el combo
                    var clientCat = new HttpClient { BaseAddress = new Uri(_catBaseUrl) };
                    var respCat = await clientCat.GetAsync("categorias");
                    ViewBag.Categorias = JsonConvert.DeserializeObject<List<CategoriaProducto>>(await respCat.Content.ReadAsStringAsync());
                }
                return View(reg);
            }

            [HttpPost]
            public async Task<IActionResult> EdicionProductoCarta(Producto reg)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    string json = JsonConvert.SerializeObject(reg);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // PUT: api/ProductosApi/actualizar
                    await client.PutAsync("actualizar", content);
                }
                return RedirectToAction("ListadoProductosCarta");
            }

            // ==========================================================
            // 5. DESACTIVAR PRODUCTO (Borrado Lógico)
            // ==========================================================
            public async Task<IActionResult> DesactivarProductoCarta(int id)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    // DELETE: api/ProductosApi/desactivar/{id}
                    HttpResponseMessage response = await client.DeleteAsync($"desactivar/{id}");
                    TempData["Mensaje"] = await response.Content.ReadAsStringAsync();
                }
                return RedirectToAction("ListadoProductosCarta");
            }

            // Acción por defecto
            public IActionResult Index() => RedirectToAction("ListadoProductosCarta");
        }
    }
