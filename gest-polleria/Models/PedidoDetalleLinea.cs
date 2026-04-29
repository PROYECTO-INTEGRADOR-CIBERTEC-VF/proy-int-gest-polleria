namespace gest_polleria.Models
{
    public class PedidoDetalleLinea
    {
        public int IdPedidoDetalle { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public string Observacion { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
