using gest_polleria.DAO;
using gest_polleria.Models;
using Microsoft.AspNetCore.Mvc;

namespace gest_polleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly AuthDAO _auth = new AuthDAO();

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await Task.Run(() => _auth.login(request));
            return Ok(response);
        }
    }
}
