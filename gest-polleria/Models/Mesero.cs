namespace gest_polleria.Models
{
    public class Mesero
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public string Rol { get; set; } = "Mesero";
        public string? Turno { get; set; }
        public string? Zona { get; set; }
    }
}
