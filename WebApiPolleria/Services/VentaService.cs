using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.DAO;
using WebApiPolleria.Models;

namespace WebApiPolleria.Services
{

    public class VentaService
    {
       
        private readonly VentaDAO _ventaDao = new VentaDAO();



        public string GenerarElectronicoConPdf(GeneracionElectronica g)
        {
            // 1) Obtener venta lista para PDF
            VentaPdfModel? pdf = _ventaDao.obtenerVentaParaPdf(g.IdVenta);
            if (pdf == null)
                return "La venta no existe";

            // 2) Generar PDF
            string rutaRelativa = $"pdf/venta_{g.IdVenta}.pdf";
            string rutaFisica = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                rutaRelativa
            );

            ComprobantePdf.Generar(rutaFisica, pdf);

            // 3) Guardar rutas / estado (RF-030)
            return _ventaDao.generarElectronico(g);
        }

        public string EnviarSunatSimulado(int idVenta)
        {
            return _ventaDao.enviarSunat(idVenta);
        }

        public string RecibirCdrSimulado(int idVenta)
        {
            return _ventaDao.recibirCdr(idVenta);
        }

        public object EnviarCliente(int idVenta)
        {
            var result = _ventaDao.enviarCliente(idVenta);

            return new
            {
                ok = result.ok,
                mensaje = result.mensaje,
                linkPdf = result.rutaPdf
            };
        }






    }
}
