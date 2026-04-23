namespace gest_polleria.Models
{
    public class PedidDetalle
    {
        public int IdPedidoDetalle { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public decimal Cantidad { get; set; }
        public string? Observacion { get; set; }
    }
}
