using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class CatalogosApiController : ControllerBase
    {
        CatalogosDAO _cat = new CatalogosDAO();

        // GET: api/CatalogosApi/roles
        [HttpGet("roles")]
        public async Task<IEnumerable<Rol>> roles()
        {
            return await Task.Run(() => _cat.listarRoles());
        }

        // GET: api/CatalogosApi/categorias
        [HttpGet("categorias")]
        public async Task<IEnumerable<CategoriaProducto>> categorias()
        {
            return await Task.Run(() => _cat.listarCategorias());
        }

        // GET: api/CatalogosApi/unidades
        [HttpGet("unidades")]
        public async Task<IEnumerable<UnidadMedida>> unidades()
        {
            return await Task.Run(() => _cat.listarUnidades());
        }
    }
}
