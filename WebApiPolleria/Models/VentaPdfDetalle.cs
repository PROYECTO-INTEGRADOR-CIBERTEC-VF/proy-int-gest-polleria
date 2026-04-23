namespace WebApiPolleria.Models
{
    public class VentaPdfDetalle
    {
        public string Producto { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
