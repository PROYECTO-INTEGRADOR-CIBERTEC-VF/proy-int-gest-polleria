namespace gest_polleria.Models
{
    public class ReporteMesero
    {
        public int IdMesero { get; set; }
        public string Mesero { get; set; } = "";
        public int TotalPedidos { get; set; }
        public decimal TotalVendido { get; set; }
    }
}
