namespace gest_polleria.Models
{
    public class CompraDetalle
    {
        public int IdCompraDetalle { get; set; }

        public int IdCompra { get; set; }

        public int IdInsumo { get; set; }

        public decimal Cantidad { get; set; }

        public decimal CostoUnitario { get; set; }

        public decimal Subtotal { get; set; }
    }
}
