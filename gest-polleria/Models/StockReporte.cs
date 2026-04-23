namespace gest_polleria.Models
{
    public class StockReporte
    {
        public int IdInsumo { get; set; }
        public string Insumo { get; set; } = "";
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; }
        public string EstadoStock { get; set; } = "";
    }
}
