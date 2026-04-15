namespace WebApiPolleria.Models
{
    public class Mesa
    {
        public int IdMesa { get; set; }
        public int NumeroMesa { get; set; }
        public string? Descripcion { get; set; }
        public int Capacidad { get; set; }
        public bool Activa { get; set; }
    }
}
