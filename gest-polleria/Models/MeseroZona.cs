namespace gest_polleria.Models
{
    public class MeseroZona
    {
        public int IdMeseroZona { get; set; }
        public int IdUsuario { get; set; }
        public int IdZona { get; set; }
        public string Zona { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
