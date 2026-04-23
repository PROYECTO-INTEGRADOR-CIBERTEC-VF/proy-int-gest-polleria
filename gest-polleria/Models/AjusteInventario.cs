namespace gest_polleria.Models

{
    public class AjusteInventario
    {
        public int IdInsumo { get; set; }
        public decimal CantidadAjuste { get; set; }
        public string Motivo { get; set; } = "";
        public int IdUsuario { get; set; }
    }

}

