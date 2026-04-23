namespace WebApiPolleria.Models
{
    public class Caja
    {
        public int IdCajaTurno { get; set; }

        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }

        public decimal MontoApertura { get; set; }
        public decimal? MontoCierre { get; set; }

        public int IdUsuarioApertura { get; set; }
        public int? IdUsuarioCierre { get; set; }

        public string? Observacion { get; set; }
    }
}
