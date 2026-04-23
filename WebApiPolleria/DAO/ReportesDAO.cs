using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class ReportesDAO
    {
        string cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn") ?? "";

        public IEnumerable<ReporteVentas> ventas(DateTime ini, DateTime fin)
        {
            var lista = new List<ReporteVentas>();
            using SqlConnection cn = new(cadena);
            using SqlCommand cmd = new("usp_ReporteVentas_RangoFechas", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FechaInicio", ini);
            cmd.Parameters.AddWithValue("@FechaFin", fin);

            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new ReporteVentas
                {
                    Fecha = (DateTime)dr["Fecha"],
                    TotalPedidos = (int)dr["TotalPedidos"],
                    TotalVentas = (decimal)dr["TotalVentas"]
                });
            }
            return lista;
        }

        public IEnumerable<ProductoMasVendido> productosMasVendidos(DateTime ini, DateTime fin)
        {
            var lista = new List<ProductoMasVendido>();
            using SqlConnection cn = new(cadena);
            using SqlCommand cmd = new("usp_Reporte_ProductosMasVendidos", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FechaInicio", ini);
            cmd.Parameters.AddWithValue("@FechaFin", fin);

            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new ProductoMasVendido
                {
                    IdProducto = (int)dr["IdProducto"],
                    Producto = dr["Producto"].ToString()!,
                    CantidadVendida = (decimal)dr["CantidadVendida"],
                    TotalVendido = (decimal)dr["TotalVendido"]
                });
            }
            return lista;
        }

        public IEnumerable<ConsumoInsumo> consumo(DateTime ini, DateTime fin)
        {
            var lista = new List<ConsumoInsumo>();
            using SqlConnection cn = new(cadena);
            using SqlCommand cmd = new("usp_Reporte_ConsumoInsumos", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FechaInicio", ini);
            cmd.Parameters.AddWithValue("@FechaFin", fin);

            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new ConsumoInsumo
                {
                    IdInsumo = (int)dr["IdInsumo"],
                    Insumo = dr["Insumo"].ToString()!,
                    CantidadConsumida = (decimal)dr["CantidadConsumida"]
                });
            }
            return lista;
        }

        public IEnumerable<StockReporte> stock()
        {
            var lista = new List<StockReporte>();
            using SqlConnection cn = new(cadena);
            using SqlCommand cmd = new("usp_Reporte_StockActual", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new StockReporte
                {
                    IdInsumo = (int)dr["IdInsumo"],
                    Insumo = dr["Insumo"].ToString()!,
                    StockActual = (decimal)dr["StockActual"],
                    StockMinimo = (decimal)dr["StockMinimo"],
                    EstadoStock = dr["EstadoStock"].ToString()!
                });
            }
            return lista;
        }

        public IEnumerable<ReporteMesero> porMesero(DateTime ini, DateTime fin)
        {
            var lista = new List<ReporteMesero>();
            using SqlConnection cn = new(cadena);
            using SqlCommand cmd = new("usp_Reporte_PorMesero", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FechaInicio", ini);
            cmd.Parameters.AddWithValue("@FechaFin", fin);

            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new ReporteMesero
                {
                    IdMesero = (int)dr["IdMesero"],
                    Mesero = dr["Mesero"].ToString()!,
                    TotalPedidos = (int)dr["TotalPedidos"],
                    TotalVendido = (decimal)dr["TotalVendido"]
                });
            }
            return lista;
        }
    }
}
