using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gest_polleria_front.Models
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Mesas = new List<MesaDto>();
            AlertasStock = new List<StockReporteDto>();
            ReporteDiario = new List<DemandaDiariaReporteDto>();
        }

        public string NombreCompleto { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int TotalClientes { get; set; }
        public int TotalMesas { get; set; }
        public int MesasLibres { get; set; }
        public int MesasOcupadas { get; set; }
        public int TotalMeserosActivos { get; set; }
        public int TotalAlertasStock { get; set; }
        public IList<MesaDto> Mesas { get; set; }
        public IList<StockReporteDto> AlertasStock { get; set; }
        public IList<DemandaDiariaReporteDto> ReporteDiario { get; set; }
    }

    public class ClientesIndexViewModel
    {
        public ClientesIndexViewModel()
        {
            Clientes = new List<ClienteDto>();
            NuevoCliente = new ClienteFormViewModel();
        }

        public string Buscar { get; set; } = string.Empty;
        public bool? EsEmpresa { get; set; }
        public IList<ClienteDto> Clientes { get; set; }
        public ClienteFormViewModel NuevoCliente { get; set; }
    }

    public class MesasIndexViewModel
    {
        public MesasIndexViewModel()
        {
            Mesas = new List<MesaDto>();
            NuevaMesa = new MesaFormViewModel();
            CambioEstado = new EstadoMesaViewModel();
        }

        public IList<MesaDto> Mesas { get; set; }
        public MesaFormViewModel NuevaMesa { get; set; }
        public EstadoMesaViewModel CambioEstado { get; set; }
    }

    public class PedidosIndexViewModel
    {
        public PedidosIndexViewModel()
        {
            Mesas = new List<MesaDto>();
            Meseros = new List<MeseroDto>();
            PedidosAbiertos = new List<PedidoAbiertoDto>();
            PedidosOperativos = new List<PedidoOperativoDto>();
            Carta = new List<ProductoCartaDto>();
            Apertura = new AbrirMesaViewModel();
            Detalle = new PedidoDetalleFormViewModel();
            Gestion = new GestionPedidoViewModel();
        }

        public IList<MesaDto> Mesas { get; set; }
        public IList<MeseroDto> Meseros { get; set; }
        public IList<PedidoAbiertoDto> PedidosAbiertos { get; set; }
        public IList<PedidoOperativoDto> PedidosOperativos { get; set; }
        public IList<ProductoCartaDto> Carta { get; set; }
        public AbrirMesaViewModel Apertura { get; set; }
        public PedidoDetalleFormViewModel Detalle { get; set; }
        public GestionPedidoViewModel Gestion { get; set; }
    }

    public class InsumosIndexViewModel
    {
        public InsumosIndexViewModel()
        {
            PorcentajeAlerta = 10;
            StockItems = new List<StockReporteDto>();
            NuevoInsumo = new InsumoFormViewModel();
        }

        public bool MostrarAlertas { get; set; }
        public decimal PorcentajeAlerta { get; set; }
        public IList<StockReporteDto> StockItems { get; set; }
        public InsumoFormViewModel NuevoInsumo { get; set; }
    }

    public class MeserosIndexViewModel
    {
        public MeserosIndexViewModel()
        {
            Meseros = new List<MeseroDto>();
            NuevoMesero = new MeseroFormViewModel();
            NuevoTurno = new MeseroTurnoFormViewModel();
            NuevaZona = new MeseroZonaFormViewModel();
        }

        public IList<MeseroDto> Meseros { get; set; }
        public MeseroFormViewModel NuevoMesero { get; set; }
        public MeseroTurnoFormViewModel NuevoTurno { get; set; }
        public MeseroZonaFormViewModel NuevaZona { get; set; }
    }

    public class ReportesIndexViewModel
    {
        public ReportesIndexViewModel()
        {
            TipoConsulta = "diario";
            Fecha = DateTime.Today;
            FechaInicio = DateTime.Today;
            FechaFin = DateTime.Today;
            Resultados = new List<DemandaDiariaReporteDto>();
        }

        public string TipoConsulta { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TotalPedidos { get; set; }
        public decimal TotalEstimado { get; set; }
        public IList<DemandaDiariaReporteDto> Resultados { get; set; }
    }

    public class ClienteFormViewModel : IValidatableObject
    {
        public ClienteFormViewModel()
        {
            TipoDocumento = "DNI";
            NumeroDocumento = string.Empty;
            RazonSocial = string.Empty;
            Nombres = string.Empty;
            Apellidos = string.Empty;
            Direccion = string.Empty;
            Telefono = string.Empty;
            Email = string.Empty;
        }

        [Required(ErrorMessage = "Selecciona el tipo de documento.")]
        public string TipoDocumento { get; set; }

        [Required(ErrorMessage = "El numero de documento es obligatorio.")]
        public string NumeroDocumento { get; set; }

        public bool EsEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
        public string Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EsEmpresa)
            {
                if (string.IsNullOrWhiteSpace(RazonSocial))
                {
                    yield return new ValidationResult(
                        "La razon social es obligatoria para clientes empresa.",
                        new[] { "RazonSocial" });
                }

                yield break;
            }

            if (string.IsNullOrWhiteSpace(Nombres))
            {
                yield return new ValidationResult(
                    "Los nombres son obligatorios para clientes persona.",
                    new[] { "Nombres" });
            }
        }
    }

    public class MesaFormViewModel
    {
        public MesaFormViewModel()
        {
            Descripcion = string.Empty;
            Capacidad = 4;
        }

        [Range(1, int.MaxValue, ErrorMessage = "Ingresa un número de mesa válido.")]
        public int NumeroMesa { get; set; }

        public string Descripcion { get; set; }

        [Range(1, 100, ErrorMessage = "La capacidad debe ser mayor a cero.")]
        public int Capacidad { get; set; }
    }

    public class EstadoMesaViewModel
    {
        public EstadoMesaViewModel()
        {
            Estado = "LIBRE";
        }

        [Range(1, int.MaxValue, ErrorMessage = "Indica la mesa a actualizar.")]
        public int IdMesa { get; set; }

        [Required(ErrorMessage = "Selecciona el estado de la mesa.")]
        public string Estado { get; set; }
    }

    public class AbrirMesaViewModel
    {
        public AbrirMesaViewModel()
        {
            IdTipoPedido = 1;
        }

        [Range(1, int.MaxValue, ErrorMessage = "Indica la mesa a abrir.")]
        public int IdMesa { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Indica el tipo de pedido.")]
        public int IdTipoPedido { get; set; }

        public int? IdMesero { get; set; }
    }

    public class PedidoDetalleFormViewModel
    {
        public PedidoDetalleFormViewModel()
        {
            IdPedidoDetalle = 0;
            Cantidad = 1;
            Observacion = string.Empty;
        }

        public int IdPedidoDetalle { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Indica el pedido.")]
        public int IdPedido { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Indica el producto.")]
        public int IdProducto { get; set; }

        [Range(typeof(decimal), "0.01", "99999", ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public decimal Cantidad { get; set; }

        public string Observacion { get; set; }
    }

    public class GestionPedidoViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Indica el pedido a gestionar.")]
        public int IdPedido { get; set; }
    }

    public class InsumoFormViewModel
    {
        public InsumoFormViewModel()
        {
            Nombre = string.Empty;
            IdUnidadMedida = 1;
            StockMinimo = 10;
            StockActual = 20;
        }

        [Required(ErrorMessage = "El nombre del insumo es obligatorio.")]
        public string Nombre { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona una unidad de medida.")]
        public int IdUnidadMedida { get; set; }

        [Range(typeof(decimal), "0", "999999", ErrorMessage = "Ingresa un stock mínimo válido.")]
        public decimal StockMinimo { get; set; }

        [Range(typeof(decimal), "0", "999999", ErrorMessage = "Ingresa un stock actual válido.")]
        public decimal StockActual { get; set; }
    }

    public class MeseroFormViewModel
    {
        public MeseroFormViewModel()
        {
            UserName = string.Empty;
            ClaveHash = string.Empty;
            NombreCompleto = string.Empty;
            Email = string.Empty;
            Telefono = string.Empty;
        }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string ClaveHash { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string NombreCompleto { get; set; }

        [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
        public string Email { get; set; }

        public string Telefono { get; set; }
    }

    public class MeseroTurnoFormViewModel
    {
        public MeseroTurnoFormViewModel()
        {
            Turno = "MANANA";
            HoraInicio = "08:00";
            HoraFin = "16:00";
        }

        [Range(1, int.MaxValue, ErrorMessage = "Indica el usuario del mesero.")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Selecciona el turno.")]
        public string Turno { get; set; }

        [Required(ErrorMessage = "Indica la hora de inicio.")]
        public string HoraInicio { get; set; }

        [Required(ErrorMessage = "Indica la hora de fin.")]
        public string HoraFin { get; set; }
    }

    public class MeseroZonaFormViewModel
    {
        public MeseroZonaFormViewModel()
        {
            IdZona = 1;
        }

        [Range(1, int.MaxValue, ErrorMessage = "Indica el usuario del mesero.")]
        public int IdUsuario { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona la zona.")]
        public int IdZona { get; set; }
    }

    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EsEmpresa { get; set; }

        public string NombreDisplay
        {
            get
            {
                if (EsEmpresa && !string.IsNullOrWhiteSpace(RazonSocial))
                {
                    return RazonSocial;
                }

                return string.Format("{0} {1}", Nombres, Apellidos).Trim();
            }
        }
    }

    public class MesaDto
    {
        public int IdMesa { get; set; }
        public int NumeroMesa { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public bool Activa { get; set; }
        public string Estado { get; set; } = string.Empty;

        public string EstadoCssClass
        {
            get
            {
                var estado = (Estado ?? string.Empty).ToUpperInvariant();
                if (estado.Contains("LIBRE"))
                {
                    return "badge-success";
                }

                if (estado.Contains("OCUP"))
                {
                    return "badge-warn";
                }

                return "badge-neutral";
            }
        }
    }

    public class MeseroDto
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public string Rol { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public string Zona { get; set; } = string.Empty;
    }

    public class StockReporteDto
    {
        public int IdInsumo { get; set; }
        public string Insumo { get; set; } = string.Empty;
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; }
        public string EstadoStock { get; set; } = string.Empty;

        public string EstadoCssClass
        {
            get
            {
                var estado = (EstadoStock ?? string.Empty).ToUpperInvariant();
                if (estado.Contains("CRIT") || estado.Contains("SIN"))
                {
                    return "badge-danger";
                }

                if (estado.Contains("BAJ"))
                {
                    return "badge-warn";
                }

                return "badge-success";
            }
        }
    }

    public class PedidoAbiertoDto
    {
        public int IdPedido { get; set; }
        public int IdMesa { get; set; }
        public int NumeroMesa { get; set; }
        public string EstadoPedido { get; set; } = string.Empty;
    }

    public class PedidoOperativoDto
    {
        public int IdPedido { get; set; }
        public string FechaHora { get; set; } = string.Empty;
        public int? NumeroMesa { get; set; }
        public string TipoPedido { get; set; } = string.Empty;
        public string EstadoPedido { get; set; } = string.Empty;
        public decimal TotalEstimado { get; set; }
        public string Mesero { get; set; } = string.Empty;
    }

    public class PedidoDetalleLineaDto
    {
        public int IdPedidoDetalle { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public string Observacion { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class ProductoCartaDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public bool ParaDelivery { get; set; }
    }

    public class DemandaDiariaReporteDto
    {
        public DateTime Fecha { get; set; }
        public int? IdMesa { get; set; }
        public int? NumeroMesa { get; set; }
        public int CantidadPedidos { get; set; }
        public decimal TotalEstimado { get; set; }
    }
}