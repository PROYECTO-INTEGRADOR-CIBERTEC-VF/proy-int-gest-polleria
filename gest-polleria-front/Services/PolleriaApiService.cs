using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using gest_polleria_front.Models;
using Newtonsoft.Json;

namespace gest_polleria_front.Services
{
    public class PolleriaApiService
    {
        private static readonly Lazy<HttpClient> LazyClient = new Lazy<HttpClient>(CreateClient);

        private static HttpClient Client
        {
            get { return LazyClient.Value; }
        }

        public async Task<LoginApiResponse> LoginAsync(string userName, string claveHash)
        {
            return await SendAsync<LoginApiResponse>(
                "AuthApi/login",
                HttpMethod.Post,
                new
                {
                    UserName = userName,
                    ClaveHash = claveHash
                });
        }

        public async Task<IList<ClienteDto>> GetClientesAsync(bool? esEmpresa, string buscar)
        {
            return await GetAsync<List<ClienteDto>>(
                       "ClientesApi/listar",
                       new
                       {
                           esEmpresa,
                           buscar
                       }) ?? new List<ClienteDto>();
        }

        public async Task<string> CreateClienteAsync(ClienteFormViewModel model)
        {
            return await SendForStringAsync(
                "ClientesApi/registrar",
                HttpMethod.Post,
                new
                {
                    model.TipoDocumento,
                    model.NumeroDocumento,
                    model.RazonSocial,
                    model.Nombres,
                    model.Apellidos,
                    model.Direccion,
                    model.Telefono,
                    model.Email,
                    model.EsEmpresa
                });
        }

        public async Task<IList<MesaDto>> GetMesasAsync(bool soloActivas)
        {
            return await GetAsync<List<MesaDto>>(
                       "MesasApi/listar",
                       new
                       {
                           soloActivas
                       }) ?? new List<MesaDto>();
        }

        public async Task<string> CreateMesaAsync(MesaFormViewModel model)
        {
            return await SendForStringAsync(
                "MesasApi/registrar",
                HttpMethod.Post,
                new
                {
                    model.NumeroMesa,
                    model.Descripcion,
                    model.Capacidad,
                    Activa = true,
                    Estado = "LIBRE"
                });
        }

        public async Task<string> ChangeMesaStateAsync(EstadoMesaViewModel model)
        {
            return await SendForStringAsync("MesasApi/estado", HttpMethod.Put, model);
        }

        public async Task<string> AbrirMesaAsync(AbrirMesaViewModel model)
        {
            return await SendForStringAsync(
                "PedidoApi/abrirmesa",
                HttpMethod.Post,
                null,
                new
                {
                    model.IdMesa,
                    model.IdTipoPedido,
                    model.IdMesero
                });
        }

        public async Task<string> AgregarDetalleAsync(PedidoDetalleFormViewModel model)
        {
            return await SendForStringAsync(
                "PedidoApi/agregardetalle",
                HttpMethod.Post,
                null,
                new
                {
                    model.IdPedido,
                    model.IdProducto,
                    model.Cantidad,
                    model.Observacion
                });
        }

        public async Task<string> ActualizarDetalleAsync(PedidoDetalleFormViewModel model)
        {
            return await SendForStringAsync(
                "PedidoApi/actualizardetalle",
                HttpMethod.Put,
                null,
                new
                {
                    model.IdPedidoDetalle,
                    model.Cantidad,
                    model.Observacion
                });
        }

        public async Task<string> EliminarDetalleAsync(int idPedidoDetalle)
        {
            return await SendForStringAsync("PedidoApi/eliminardetalle/" + idPedidoDetalle, HttpMethod.Delete);
        }

        public async Task<string> EnviarPedidoACocinaAsync(int idPedido)
        {
            return await SendForStringAsync("PedidoApi/enviaracocina/" + idPedido, HttpMethod.Put);
        }

        public async Task<string> CerrarMesaAsync(int idPedido)
        {
            return await SendForStringAsync("PedidoApi/cerrarmesa/" + idPedido, HttpMethod.Put);
        }

        public async Task<IList<PedidoAbiertoDto>> GetPedidosAbiertosAsync()
        {
            return await GetAsync<List<PedidoAbiertoDto>>("PedidoApi/abiertos") ?? new List<PedidoAbiertoDto>();
        }

        public async Task<IList<PedidoOperativoDto>> GetPedidosOperativosAsync()
        {
            return await GetAsync<List<PedidoOperativoDto>>("PedidoApi/listar") ?? new List<PedidoOperativoDto>();
        }

        public async Task<IList<ProductoCartaDto>> GetCartaAsync()
        {
            return await GetAsync<List<ProductoCartaDto>>("PedidoApi/carta") ?? new List<ProductoCartaDto>();
        }

        public async Task<IList<PedidoDetalleLineaDto>> GetPedidoDetalleAsync(int idPedido)
        {
            return await GetAsync<List<PedidoDetalleLineaDto>>("PedidoApi/detalle/" + idPedido) ?? new List<PedidoDetalleLineaDto>();
        }

        public async Task<IList<StockReporteDto>> GetReporteStockAsync()
        {
            return await GetAsync<List<StockReporteDto>>("InsumosApi/reporte-stock") ?? new List<StockReporteDto>();
        }

        public async Task<IList<StockReporteDto>> GetAlertasStockAsync(decimal porcentaje)
        {
            return await GetAsync<List<StockReporteDto>>(
                       "InsumosApi/alertas",
                       new
                       {
                           porcentaje
                       }) ?? new List<StockReporteDto>();
        }

        public async Task<string> CreateInsumoAsync(InsumoFormViewModel model)
        {
            return await SendForStringAsync(
                "InsumosApi/registrar",
                HttpMethod.Post,
                new
                {
                    model.Nombre,
                    model.IdUnidadMedida,
                    model.StockMinimo,
                    model.StockActual,
                    Activo = true
                });
        }

        public async Task<IList<MeseroDto>> GetMeserosAsync()
        {
            return await GetAsync<List<MeseroDto>>("MeserosApi/listar") ?? new List<MeseroDto>();
        }

        public async Task<string> CreateMeseroAsync(MeseroFormViewModel model)
        {
            return await SendForStringAsync(
                "MeserosApi/registrar",
                HttpMethod.Post,
                new
                {
                    model.UserName,
                    model.ClaveHash,
                    model.NombreCompleto,
                    model.Email,
                    model.Telefono
                });
        }

        public async Task<string> RegistrarTurnoAsync(MeseroTurnoFormViewModel model)
        {
            return await SendForStringAsync(
                "MeserosApi/turnos/registrar",
                HttpMethod.Post,
                new
                {
                    model.IdUsuario,
                    model.Turno,
                    HoraInicio = NormalizeTime(model.HoraInicio),
                    HoraFin = NormalizeTime(model.HoraFin),
                    Activo = true
                });
        }

        public async Task<string> AsignarZonaAsync(MeseroZonaFormViewModel model)
        {
            return await SendForStringAsync(
                "MeserosApi/zonas/asignar",
                HttpMethod.Post,
                new
                {
                    model.IdUsuario,
                    model.IdZona
                });
        }

        public async Task<IList<DemandaDiariaReporteDto>> GetDemandaDiariaAsync(DateTime fecha)
        {
            return await GetAsync<List<DemandaDiariaReporteDto>>(
                       "ReportesApi/demanda-diaria",
                       new
                       {
                           fecha
                       }) ?? new List<DemandaDiariaReporteDto>();
        }

        public async Task<IList<DemandaDiariaReporteDto>> GetDemandaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await GetAsync<List<DemandaDiariaReporteDto>>(
                       "ReportesApi/demanda-rango",
                       new
                       {
                           fechaInicio,
                           fechaFin
                       }) ?? new List<DemandaDiariaReporteDto>();
        }

        private static HttpClient CreateClient()
        {
            var baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "https://localhost:7208/api";
            if (!baseUrl.EndsWith("/", StringComparison.Ordinal))
            {
                baseUrl += "/";
            }

            var handler = new HttpClientHandler();

            if (baseUrl.IndexOf("localhost", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                handler.ServerCertificateCustomValidationCallback = (request, certificate, chain, errors) => true;
            }

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(20)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private async Task<T> GetAsync<T>(string path, object query = null)
        {
            var response = await Client.GetAsync(BuildUrl(path, query));
            return await ReadAsync<T>(response);
        }

        private async Task<T> SendAsync<T>(string path, HttpMethod method, object body = null, object query = null)
        {
            using (var request = new HttpRequestMessage(method, BuildUrl(path, query)))
            {
                if (body != null)
                {
                    request.Content = CreateJsonContent(body);
                }

                var response = await Client.SendAsync(request);
                return await ReadAsync<T>(response);
            }
        }

        private async Task<string> SendForStringAsync(string path, HttpMethod method, object body = null, object query = null)
        {
            using (var request = new HttpRequestMessage(method, BuildUrl(path, query)))
            {
                if (body != null)
                {
                    request.Content = CreateJsonContent(body);
                }

                var response = await Client.SendAsync(request);
                return await ReadStringAsync(response);
            }
        }

        private static StringContent CreateJsonContent(object body)
        {
            return new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        private static async Task<T> ReadAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw BuildException(response, content);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(content);
        }

        private static async Task<string> ReadStringAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw BuildException(response, content);
            }

            return string.IsNullOrWhiteSpace(content) ? "Operacion completada." : content;
        }

        private static Exception BuildException(HttpResponseMessage response, string content)
        {
            var message = string.IsNullOrWhiteSpace(content) ? response.ReasonPhrase : content;
            return new InvalidOperationException("API " + (int)response.StatusCode + ": " + message);
        }

        private static string BuildUrl(string path, object query)
        {
            if (query == null)
            {
                return path;
            }

            var collection = HttpUtility.ParseQueryString(string.Empty);
            foreach (var property in query.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var value = property.GetValue(query, null);
                if (value == null)
                {
                    continue;
                }

                var text = SerializeQueryValue(value);
                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }

                collection[property.Name] = text;
            }

            var queryString = collection.ToString();
            return string.IsNullOrWhiteSpace(queryString) ? path : path + "?" + queryString;
        }

        private static string SerializeQueryValue(object value)
        {
            if (value is DateTime)
            {
                return ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            if (value is bool)
            {
                return value.ToString().ToLowerInvariant();
            }

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private static string NormalizeTime(string value)
        {
            TimeSpan result;
            if (TimeSpan.TryParse(value, out result))
            {
                return result.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);
            }

            return value;
        }
    }
}
