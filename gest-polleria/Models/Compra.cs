namespace gest_polleria.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdProveedor { get; set; }
        public string TipoComprobante { get; set; } = string.Empty;
        public string? Serie { get; set; }
        public string? Numero { get; set; }
        public decimal ImporteTotal { get; set; }
    }
}
