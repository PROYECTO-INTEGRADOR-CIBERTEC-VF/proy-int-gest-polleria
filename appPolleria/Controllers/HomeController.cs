using appPolleria.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace appPolleria.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBase = "https://localhost:7208/api";

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_apiBase);
        }

        // GET: /Home/Index  →  muestra el formulario
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Home/Login  →  procesa el formulario
        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string contrasena)
        {
            var loginData = new LoginRequest
            {
                UserName = usuario,
                ClaveHash = contrasena
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PostAsync("/api/AuthApi/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View("Index");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!loginResponse.Ok)
            {
                ViewBag.Error = loginResponse.Mensaje;
                return View("Index");
            }

            // Guardar datos en sesión
            HttpContext.Session.SetString("Usuario", loginResponse.NombreCompleto ?? "");
            HttpContext.Session.SetString("Rol", loginResponse.Rol ?? "");

            // Redirigir según el rol
            if (loginResponse.Rol == "Administrador")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "Mozo"); return RedirectToAction("Index", "Mozo");
        }

        // GET: /Home/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}