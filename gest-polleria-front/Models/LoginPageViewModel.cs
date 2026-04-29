using System.ComponentModel.DataAnnotations;

namespace gest_polleria_front.Models
{
    public class LoginPageViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La clave es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        public string ClaveHash { get; set; } = string.Empty;
    }

    public class LoginApiResponse
    {
        public bool Ok { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int? IdUsuario { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
