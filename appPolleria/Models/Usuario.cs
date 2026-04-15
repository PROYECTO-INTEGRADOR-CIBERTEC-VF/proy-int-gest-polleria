using System.Text.Json.Serialization;

namespace appPolleria.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = default!;
        [JsonIgnore]
        public string ClaveHash { get; set; } = default!;
        public string NombreCompleto { get; set; } = default!;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
