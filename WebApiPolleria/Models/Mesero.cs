namespace WebApiPolleria.Models
{
    public class Mesero
    {
        public int IdMesero { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }
        public string Turno { get; set; }
        public bool Activo { get; set; } = true;
    }
}