namespace gest_polleria.Models
{
    public class AsignacionMesero
    {
        public int IdAsignacion { get; set; }
        public int IdMesero { get; set; }
        public int IdMesa { get; set; }
        public DateTime FechaAsignacion { get; set; }

        public string? NombreMesero { get; set; }
        public string? NumeroMesa { get; set; }
        public string? NombreZona { get; set; }
    }
}