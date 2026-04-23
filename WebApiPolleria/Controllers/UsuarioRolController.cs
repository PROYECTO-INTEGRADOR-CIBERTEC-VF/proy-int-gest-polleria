using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

[ApiController]
[Route("api/[controller]")]
public class UsuarioRolController : ControllerBase
{
    UsuarioRolDAO _usuario = new UsuarioRolDAO();

    [HttpPost("asignar")]
    public async Task<string> asignar([FromBody] UsuarioRol ur)
    {
        return await Task.Run(() =>
            _usuario.asignarRol(ur.IdUsuario, ur.IdRol)
        );
    }

    [HttpDelete("quitar")]
    public async Task<string> quitar([FromBody] UsuarioRol ur)
    {
        return await Task.Run(() =>
            _usuario.quitarRol(ur.IdUsuario, ur.IdRol)
        );
    }

    [HttpGet("listar/{idUsuario}")]
    public async Task<IEnumerable<UsuarioRol>> listar(int idUsuario)
    {
        return await Task.Run(() =>
            _usuario.listarPorUsuario(idUsuario)
        );
    }


}
