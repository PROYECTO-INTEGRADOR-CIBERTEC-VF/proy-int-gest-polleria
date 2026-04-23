namespace gest_polleria.Models
{
    public class CategoriaProducto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = default!;
        public string? Descripcion { get; set; }
    }
}