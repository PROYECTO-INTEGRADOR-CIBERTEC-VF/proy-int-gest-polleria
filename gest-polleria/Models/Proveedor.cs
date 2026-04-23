namespace gest_polleria.Models

public class Proveedor
{
    public int IdProveedor { get; set; }
    public string Ruc { get; set; } = default!;
    public string RazonSocial { get; set; } = default!;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Contacto { get; set; }
    public bool Activo { get; set; }
}