namespace gest_polleria.Models
{
    public class MeseroRegistroRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string ClaveHash { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}
