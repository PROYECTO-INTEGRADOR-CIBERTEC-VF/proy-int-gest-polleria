using gest_polleria.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gest_polleria.DAO
{
    public class ReporteDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<DemandaDiariaReporte> demandaDiaria(DateTime fecha)
        {
            var lista = new List<DemandaDiariaReporte>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    ;WITH TotalesPedido AS
                    (
                        SELECT
                            pd.IdPedido,
                            SUM(CAST(pd.Cantidad AS DECIMAL(18,2)) * pr.PrecioVenta) AS TotalPedido
                        FROM dbo.PedidosDetalle pd
                        INNER JOIN dbo.Productos pr ON pr.IdProducto = pd.IdProducto
                        WHERE pd.Activo = 1
                        GROUP BY pd.IdPedido
                    )
                    SELECT
                        CAST(p.FechaHora AS DATE) AS Fecha,
                        p.IdMesa,
                        m.NumeroMesa,
                        COUNT(1) AS CantidadPedidos,
                        SUM(ISNULL(tp.TotalPedido, 0)) AS TotalEstimado
                    FROM dbo.Pedidos p
                    LEFT JOIN dbo.Mesas m ON m.IdMesa = p.IdMesa
                    LEFT JOIN TotalesPedido tp ON tp.IdPedido = p.IdPedido
                    WHERE CAST(p.FechaHora AS DATE) = @Fecha
                    GROUP BY CAST(p.FechaHora AS DATE), p.IdMesa, m.NumeroMesa
                    ORDER BY m.NumeroMesa", cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Date;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DemandaDiariaReporte
                            {
                                Fecha = dr.GetDateTime(dr.GetOrdinal("Fecha")),
                                IdMesa = dr["IdMesa"] == DBNull.Value ? null : dr.GetInt32(dr.GetOrdinal("IdMesa")),
                                NumeroMesa = dr["NumeroMesa"] == DBNull.Value ? null : dr.GetInt32(dr.GetOrdinal("NumeroMesa")),
                                CantidadPedidos = dr.GetInt32(dr.GetOrdinal("CantidadPedidos")),
                                TotalEstimado = dr.GetDecimal(dr.GetOrdinal("TotalEstimado"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public IEnumerable<DemandaDiariaReporte> demandaRango(DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<DemandaDiariaReporte>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    ;WITH TotalesPedido AS
                    (
                        SELECT
                            pd.IdPedido,
                            SUM(CAST(pd.Cantidad AS DECIMAL(18,2)) * pr.PrecioVenta) AS TotalPedido
                        FROM dbo.PedidosDetalle pd
                        INNER JOIN dbo.Productos pr ON pr.IdProducto = pd.IdProducto
                        WHERE pd.Activo = 1
                        GROUP BY pd.IdPedido
                    )
                    SELECT
                        CAST(p.FechaHora AS DATE) AS Fecha,
                        p.IdMesa,
                        m.NumeroMesa,
                        COUNT(1) AS CantidadPedidos,
                        SUM(ISNULL(tp.TotalPedido, 0)) AS TotalEstimado
                    FROM dbo.Pedidos p
                    LEFT JOIN dbo.Mesas m ON m.IdMesa = p.IdMesa
                    LEFT JOIN TotalesPedido tp ON tp.IdPedido = p.IdPedido
                    WHERE CAST(p.FechaHora AS DATE) BETWEEN @FechaInicio AND @FechaFin
                    GROUP BY CAST(p.FechaHora AS DATE), p.IdMesa, m.NumeroMesa
                    ORDER BY Fecha, m.NumeroMesa", cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = fechaInicio.Date;
                    cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = fechaFin.Date;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DemandaDiariaReporte
                            {
                                Fecha = dr.GetDateTime(dr.GetOrdinal("Fecha")),
                                IdMesa = dr["IdMesa"] == DBNull.Value ? null : dr.GetInt32(dr.GetOrdinal("IdMesa")),
                                NumeroMesa = dr["NumeroMesa"] == DBNull.Value ? null : dr.GetInt32(dr.GetOrdinal("NumeroMesa")),
                                CantidadPedidos = dr.GetInt32(dr.GetOrdinal("CantidadPedidos")),
                                TotalEstimado = dr.GetDecimal(dr.GetOrdinal("TotalEstimado"))
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}
