namespace gest_polleria.Models
{
    public class UsuarioRolDetalle
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = "";
        public string? Descripcion { get; set; }
    }
}
