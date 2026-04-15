namespace WebApiPolleria.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int IdTipoPedido { get; set; }  
        public int IdEstadoPedido { get; set; }
        public int? IdMesa { get; set; }
        public int? IdCliente { get; set; }     
        public DateTime FechaPedido { get; set; }

        public string? DireccionDelivery { get; set; }
    }

}
