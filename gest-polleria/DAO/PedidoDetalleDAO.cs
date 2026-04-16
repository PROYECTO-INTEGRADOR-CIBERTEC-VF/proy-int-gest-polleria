using Microsoft.Data.SqlClient;
using System.Data;

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
                        idPedidoDetalleNuevo = Convert.ToInt32(pId.Value);
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
                }
            }
            return mensaje;
        }

        public object buscarDetalle(int value)
        {
            throw new NotImplementedException();
        }
    }
}

