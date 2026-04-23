namespace WebApiPolleria.Models
{
    public class LoginResponse
    {
        public bool Ok { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public int? IdUsuario { get; set; }
        public string? Usuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Rol { get; set; }
    }
}
