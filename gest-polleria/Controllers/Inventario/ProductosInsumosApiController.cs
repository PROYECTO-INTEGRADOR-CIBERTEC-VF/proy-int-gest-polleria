using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace gest_polleria.Controllers.Inventario
{
    public class ProductosInsumosApiController : Controller
    {
        // URL base de la Web API para el módulo de Recetas (Productos-Insumos)
        private readonly string _apiBaseUrl = "https://localhost:7173/api/ProductosInsumosApi/";

        // RF-011: LISTADO DE RECETA POR PRODUCTO
        // Muestra qué insumos y en qué cantidad componen un producto específico
        public async Task<IActionResult> ListadoRecetaProducto(int idProducto)
        {
            List<ProductoInsumo> receta = new List<ProductoInsumo>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                // Consumimos el endpoint: GET api/ProductosInsumosApi/listar/{idProducto}
                HttpResponseMessage response = await client.GetAsync($"listar/{idProducto}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    receta = JsonConvert.DeserializeObject<List<ProductoInsumo>>(jsonResponse) ?? new List<ProductoInsumo>();
                }
            }

            // Pasamos el ID del producto a la vista para poder agregar nuevos insumos a esta receta específica
            ViewBag.IdProducto = idProducto;
            return View(receta);
        }

        // RF-011: REGISTRAR INSUMO EN RECETA (POST)
        // Permite añadir un insumo (ej: 0.250kg de Pollo) a un producto
        public IActionResult RegistrarInsumoReceta(int idProducto)
        {
            // Inicializamos el modelo con el ID del producto padre
            return View(new ProductoInsumo { IdProducto = idProducto });
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarInsumoReceta(ProductoInsumo reg)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                string json = JsonConvert.SerializeObject(reg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Consumimos el endpoint: POST api/ProductosInsumosApi/registrar
                HttpResponseMessage response = await client.PostAsync("registrar", content);
                respuesta = await response.Content.ReadAsStringAsync();
            }

            // Si el registro es exitoso, redirigimos al listado de la receta de ese producto
            // En caso contrario, mostramos el mensaje de error en la misma vista
            if (respuesta.Contains("correctamente") || respuesta.Contains("agregado"))
            {
                return RedirectToAction("ListadoRecetaProducto", new { idProducto = reg.IdProducto });
            }

            ViewBag.Mensaje = respuesta;
            return View(reg);
        }

        // RF-011: ELIMINAR INSUMO DE RECETA
        // Quita un insumo de la composición de un plato
        public async Task<IActionResult> EliminarInsumoReceta(int id, int idProducto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseUrl);
                // Consumimos el endpoint: DELETE api/ProductosInsumosApi/eliminar/{id}
                await client.DeleteAsync($"eliminar/{id}");
            }
            return RedirectToAction("ListadoRecetaProducto", new { idProducto = idProducto });
        }

        // Redirección por defecto al listado de productos si se accede al Index sin ID
        public IActionResult Index()
        {
            return RedirectToAction("ListadoProductosCarta", "ProductosApi");
        }
    }
}
