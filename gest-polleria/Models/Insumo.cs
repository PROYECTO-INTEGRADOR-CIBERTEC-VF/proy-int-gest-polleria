namespace gest_polleria.Models
{
    public class Insumo
    {
        public int IdInsumo { get; set; }
        public string Nombre { get; set; } = default!;
        public int IdUnidadMedida { get; set; }
        public decimal StockMinimo { get; set; }
        public decimal StockActual { get; set; }
        public bool Activo { get; set; }
    }

}
