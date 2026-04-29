namespace gest_polleria.Models
{
    public class DemandaDiariaReporte
    {
        public DateTime Fecha { get; set; }
        public int? IdMesa { get; set; }
        public int? NumeroMesa { get; set; }
        public int CantidadPedidos { get; set; }
        public decimal TotalEstimado { get; set; }
    }
}
