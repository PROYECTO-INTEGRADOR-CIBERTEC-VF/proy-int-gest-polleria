using Microsoft.AspNetCore.Mvc;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly AuthDAO _auth = new AuthDAO();

        // POST: api/AuthApi/login
        [HttpPost("login")]
        public async Task<LoginResponse> login([FromBody] LoginRequest req)
        {
            return await Task.Run(() =>
            {
                return _auth.login(req);
            });
        }
    }
}
