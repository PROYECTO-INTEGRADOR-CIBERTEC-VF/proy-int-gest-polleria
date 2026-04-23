namespace WebApiPolleria.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? RazonSocial { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public bool EsEmpresa { get; set; }
    }
}
