namespace gest_polleria.Models
{
    public class PedidoAbierto
    {
        public int IdPedido { get; set; }
        public int IdMesa { get; set; }
        public int NumeroMesa { get; set; }
        public string EstadoPedido { get; set; } = string.Empty;
    }
}
