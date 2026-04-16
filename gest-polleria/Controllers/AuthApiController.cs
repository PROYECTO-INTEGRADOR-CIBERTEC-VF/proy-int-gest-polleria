using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace gest_polleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("cn");
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var query = @"SELECT u.IdUsuario, u.UserName, u.NombreCompleto, r.Nombre as Rol
                             FROM Usuario u
                             INNER JOIN UsuarioRol ur ON u.IdUsuario = ur.IdUsuario
                             INNER JOIN Rol r ON ur.IdRol = r.IdRol
                             WHERE u.UserName = @UserName 
                             AND u.ClaveHash = @ClaveHash 
                             AND u.Activo = 1";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", request.UserName);
                command.Parameters.AddWithValue("@ClaveHash", request.ClaveHash);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return Ok(new LoginResponse
                    {
                        Ok = true,
                        Mensaje = "Login exitoso",
                        IdUsuario = reader.GetInt32(0),
                        Usuario = reader.GetString(1),
                        NombreCompleto = reader.GetString(2),
                        Rol = reader.GetString(3)
                    });
                }

                return Ok(new LoginResponse
                {
                    Ok = false,
                    Mensaje = "Usuario o contraseña incorrectos"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse
                {
                    Ok = false,
                    Mensaje = $"Error: {ex.Message}"
                });
            }
        }
    }
}