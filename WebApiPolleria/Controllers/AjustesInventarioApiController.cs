using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

[ApiController]
[Route("api/[controller]")]
public class AjustesInventarioApiController : ControllerBase
{
    AjusteInventarioDAO _inventario = new AjusteInventarioDAO();

    [HttpPost("registrar")]
    public async Task<string> registrar([FromBody] AjusteInventario a)
    {
        return await Task.Run(() =>
            _inventario.registrar(a)
        );
    }
}
