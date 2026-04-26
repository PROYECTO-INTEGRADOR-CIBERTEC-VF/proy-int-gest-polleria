// Proyecto: gest_polleria
// Carpeta: Models

namespace gest_polleria.Models
{
    public class Mesa
    {
        public int IdMesa { get; set; }
        public int NumeroMesa { get; set; }
        public string? Descripcion { get; set; }
        public int Capacidad { get; set; }
        public bool Activa { get; set; }
        public int? IdMesero { get; set; }
    }
}