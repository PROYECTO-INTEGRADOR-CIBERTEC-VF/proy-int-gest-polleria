using System;
using System.ComponentModel.DataAnnotations;

namespace gest_polleria.Models
{
    public class AsignacionMeseros
    {
        [Key]
        public int IdAsignacion { get; set; }

        [Required]
        public int IdMesero { get; set; }

        [Required]
        public int IdMesa { get; set; }

        public DateTime FechaAsignacion { get; set; }

        public string? NombreMesero { get; set; }
        public string? NumeroMesa { get; set; }
    }
}