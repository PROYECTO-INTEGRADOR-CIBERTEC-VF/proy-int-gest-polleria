namespace gest_polleria.Models
{
    public class PedidoOperativo
    {
        public int IdPedido { get; set; }
        public string FechaHora { get; set; } = string.Empty;
        public int? NumeroMesa { get; set; }
        public string TipoPedido { get; set; } = string.Empty;
        public string EstadoPedido { get; set; } = string.Empty;
        public decimal TotalEstimado { get; set; }
        public string Mesero { get; set; } = string.Empty;
    }
}
