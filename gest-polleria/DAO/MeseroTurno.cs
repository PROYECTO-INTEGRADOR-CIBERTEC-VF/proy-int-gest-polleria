namespace gest_polleria.Models
{
    public class MeseroTurno
    {
        public int IdMeseroTurno { get; set; }
        public int IdUsuario { get; set; }
        public string Turno { get; set; } = string.Empty;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool Activo { get; set; }
    }
}
