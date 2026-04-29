namespace gest_polleria.Models
{
    public class ProductoCarta
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public bool ParaDelivery { get; set; }
    }
}
