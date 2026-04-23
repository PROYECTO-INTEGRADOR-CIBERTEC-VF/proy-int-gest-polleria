namespace WebApiPolleria.Models
{
    public class VentaConsultaDTO
    {
        public int IdVenta { get; set; }
        public DateTime FechaHora { get; set; }
        public string Serie { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public string EstadoComprobante { get; set; } = string.Empty;
        public string? Cliente { get; set; }
    }
}
