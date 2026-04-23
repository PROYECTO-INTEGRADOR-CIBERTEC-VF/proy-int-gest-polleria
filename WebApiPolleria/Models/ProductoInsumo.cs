namespace WebApiPolleria.Models
{
    public class ProductoInsumo
    {
        public int IdProductoInsumo { get; set; }
        public int IdProducto { get; set; }   
        public int IdInsumo { get; set; }
        public decimal Cantidad { get; set; }

        public string? Insumo { get; set; }
        public string? Abreviatura { get; set; }
    }
}
