using gest_polleria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gest_polleria.DAO;
using Newtonsoft.Json;
using System.Text;

namespace gest_polleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesApiController : ControllerBase
    {
        ClienteDAO _cliente = new ClienteDAO();

        // GET: api/ClientesApi/listar?esEmpresa=true&buscar=...
        [HttpGet("listar")]
        public async Task<IEnumerable<Cliente>> listar([FromQuery] bool? esEmpresa = null, [FromQuery] string? buscar = null)
        {
            return await Task.Run(() => _cliente.listar(esEmpresa, buscar));
        }



    }
}
