namespace WebApiPolleria.Models
{
    public class VentaPago
    {
        public int IdVentaPago { get; set; }

        public int IdVenta { get; set; }
        public int IdMedioPago { get; set; }

        public decimal Monto { get; set; }
    }
}
