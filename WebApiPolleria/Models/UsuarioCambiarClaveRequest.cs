namespace WebApiPolleria.Models
{
    public class UsuarioCambiarClaveRequest
    {
        public int IdUsuario { get; set; }
        public string Clave { get; set; } = default!;
    }
}
