namespace WebApiPolleria.Models
{
    public class AlertaStock
    {
        public int IdInsumo { get; set; }
        public string Insumo { get; set; } = "";
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; }
    }
}
