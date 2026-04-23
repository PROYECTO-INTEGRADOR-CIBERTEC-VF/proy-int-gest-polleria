using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

[ApiController]
[Route("api/[controller]")]
public class AlertasStockApiController : ControllerBase
{
    AlertaStockDAO _alertas = new AlertaStockDAO();

    [HttpGet("listar")]
    public async Task<IEnumerable<AlertaStock>> listar()
    {
        return await Task.Run(() =>
            _alertas.listar()
        );
    }
}
