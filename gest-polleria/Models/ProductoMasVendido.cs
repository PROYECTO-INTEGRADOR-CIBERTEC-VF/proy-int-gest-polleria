namespace gest_polleria.Models

  public class ProductoMasVendido
{
    public int IdProducto { get; set; }
    public string Producto { get; set; } = "";
    public decimal CantidadVendida { get; set; }
    public decimal TotalVendido { get; set; }
}
