namespace gest_polleria.Models

     public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = default!;
        public int IdCategoria { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool Activo { get; set; }
        public bool EsCombo { get; set; }
        public bool ParaDelivery { get; set; }
    }
}
