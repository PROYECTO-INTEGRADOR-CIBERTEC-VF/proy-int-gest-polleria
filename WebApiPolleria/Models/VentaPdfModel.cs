namespace WebApiPolleria.Models
{
    public class VentaPdfModel
    {
        public string Serie { get; set; } = "";
        public string Numero { get; set; } = "";
        public DateTime FechaHora { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }

        public List<VentaPdfDetalle> Detalle { get; set; } = new();
    }

    
}
