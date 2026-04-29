using Microsoft.Data.SqlClient;
using System.Data;
using gest_polleria.Models;

namespace gest_polleria.DAO
{
    public class PedidoDetalleDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public string insertarDetalle(int idPedido, int idProducto, decimal cantidad, string? observacion, out int idPedidoDetalleNuevo)
        {
            idPedidoDetalleNuevo = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_PedidosDetalle_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;
                    cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;
                    cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = cantidad;
                    cmd.Parameters.Add("@Observacion", SqlDbType.NVarChar, 200).Value = (object?)observacion ?? DBNull.Value;

                    var pId = cmd.Parameters.Add("@IdPedidoDetalleNuevo", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                    {
                        idPedidoDetalleNuevo = Convert.ToInt32(pId.Value);
                        RecalcularTotalPedido(cn, idPedido);
                    }
                }
            }

            return mensaje;
        }

        public string actualizarDetalle(int idPedidoDetalle, decimal cantidad, string? observacion)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_PedidosDetalle_Actualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdPedidoDetalle", SqlDbType.Int).Value = idPedidoDetalle;
                    cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = cantidad;
                    cmd.Parameters.Add("@Observacion", SqlDbType.NVarChar, 200).Value = (object?)observacion ?? DBNull.Value;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (Convert.ToBoolean(pOk.Value))
                    {
                        int idPedido = ObtenerIdPedidoDetalle(cn, idPedidoDetalle);
                        if (idPedido > 0)
                        {
                            RecalcularTotalPedido(cn, idPedido);
                        }
                    }
                }
            }
            return mensaje;
        }

        public string eliminarDetalle(int idPedidoDetalle)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                int idPedido = ObtenerIdPedidoDetalle(cn, idPedidoDetalle);
                using (SqlCommand cmd = new SqlCommand("dbo.usp_PedidosDetalle_Eliminar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdPedidoDetalle", SqlDbType.Int).Value = idPedidoDetalle;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (Convert.ToBoolean(pOk.Value))
                    {
                        RecalcularTotalPedido(cn, idPedido);
                    }
                }
            }
            return mensaje;
        }

        private static int ObtenerIdPedidoDetalle(SqlConnection cn, int idPedidoDetalle)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 IdPedido FROM dbo.PedidosDetalle WHERE IdPedidoDetalle = @IdPedidoDetalle", cn))
            {
                cmd.Parameters.Add("@IdPedidoDetalle", SqlDbType.Int).Value = idPedidoDetalle;
                object? resultado = cmd.ExecuteScalar();
                return resultado == null || resultado == DBNull.Value ? 0 : Convert.ToInt32(resultado);
            }
        }

        private static void RecalcularTotalPedido(SqlConnection cn, int idPedido)
        {
            if (idPedido <= 0)
            {
                return;
            }

            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE p
                SET TotalEstimado = ISNULL(t.Total, 0)
                FROM dbo.Pedidos p
                OUTER APPLY
                (
                    SELECT SUM(CAST(pd.Cantidad AS DECIMAL(18,2)) * pr.PrecioVenta) AS Total
                    FROM dbo.PedidosDetalle pd
                    INNER JOIN dbo.Productos pr ON pr.IdProducto = pd.IdProducto
                    WHERE pd.IdPedido = p.IdPedido
                      AND pd.Activo = 1
                ) t
                WHERE p.IdPedido = @IdPedido", cn))
            {
                cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<PedidoDetalleLinea> listarDetallePedido(int idPedido)
        {
            List<PedidoDetalleLinea> lista = new List<PedidoDetalleLinea>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT d.IdPedidoDetalle,
                           d.IdPedido,
                           d.IdProducto,
                           ISNULL(p.Nombre, '') AS Producto,
                           d.Cantidad,
                           ISNULL(d.Observacion, '') AS Observacion,
                           p.PrecioVenta AS PrecioUnitario,
                           (d.Cantidad * p.PrecioVenta) AS Subtotal
                    FROM PedidosDetalle d
                    INNER JOIN Productos p ON p.IdProducto = d.IdProducto
                    WHERE d.IdPedido = @IdPedido
                    ORDER BY d.IdPedidoDetalle", cn))
                {
                    cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new PedidoDetalleLinea
                            {
                                IdPedidoDetalle = dr.GetInt32(0),
                                IdPedido = dr.GetInt32(1),
                                IdProducto = dr.GetInt32(2),
                                Producto = dr.GetString(3),
                                Cantidad = dr.GetDecimal(4),
                                Observacion = dr.GetString(5),
                                PrecioUnitario = dr.GetDecimal(6),
                                Subtotal = dr.GetDecimal(7)
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}


