namespace gest_polleria.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime FechaHora { get; set; }

        public int? IdPedido { get; set; }
        public int? IdCliente { get; set; }

        public int IdTipoComprobante { get; set; }
        public string Serie { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }

        public int IdUsuarioCajero { get; set; }
        public int IdCajaTurno { get; set; }
        public int IdEstadoComprobante { get; set; }

        public string? HashFirma { get; set; }
        public string? RutaPdf { get; set; }
        public string? RutaXml { get; set; }
        public string? RutaCdr { get; set; }

        public string? CodigoSunat { get; set; }
        public string? MensajeSunat { get; set; }
    }
}
