namespace WebApiPolleria.Models
{
    public class UsuarioActualizarRequest
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = default!;
        public string NombreCompleto { get; set; } = default!;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
    }
}
