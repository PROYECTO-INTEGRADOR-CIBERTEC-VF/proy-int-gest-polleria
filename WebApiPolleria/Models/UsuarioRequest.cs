namespace WebApiPolleria.Models
{
    public class UsuarioRequest
    {
        public string UserName { get; set; } = default!;
        public string Clave { get; set; } = default!;
        public string NombreCompleto { get; set; } = default!;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}
