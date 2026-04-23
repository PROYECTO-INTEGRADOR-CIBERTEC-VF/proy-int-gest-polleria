using Microsoft.Data.SqlClient;
using System.Data;


namespace WebApiPolleria.DAO
{
    public class PedidoDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public string abrirMesa(int idMesa, int idTipoPedido, int? idMesero, out int idPedidoNuevo)
        {
            idPedidoNuevo = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Pedidos_AbrirMesa", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = idMesa;
                    cmd.Parameters.Add("@IdTipoPedido", SqlDbType.Int).Value = idTipoPedido;
                    cmd.Parameters.Add("@IdMesero", SqlDbType.Int).Value = (object?)idMesero ?? DBNull.Value;

                    var pId = cmd.Parameters.Add("@IdPedidoNuevo", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idPedidoNuevo = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        public string enviarACocina(int idPedido)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Pedidos_EnviarCocina", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;

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

        public string cerrarMesa(int idPedido)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Pedidos_CerrarMesa", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;

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

        public string registrarPedido(
     int idTipoPedido,
     int? idCliente,
     string? direccion,
     out int idPedido
 )
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand("usp_Pedidos_Registrar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdTipoPedido", SqlDbType.Int).Value = idTipoPedido;
            cmd.Parameters.Add("@IdEstadoPedido", SqlDbType.Int).Value = 1; // REGISTRADO
            cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = (object?)idCliente ?? DBNull.Value;
            cmd.Parameters.Add("@DireccionDelivery", SqlDbType.NVarChar, 200)
                          .Value = (object?)direccion ?? DBNull.Value;
            cmd.Parameters.Add("@TotalEstimado", SqlDbType.Decimal).Value = 0;

            var pId = cmd.Parameters.Add("@IdPedido", SqlDbType.Int);
            pId.Direction = ParameterDirection.Output;

            cn.Open();
            cmd.ExecuteNonQuery();

            idPedido = (int)pId.Value;
            return "Pedido registrado correctamente";
        }

        public string cambiarEstado(int idPedido, int idEstado)
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand(
                "usp_Pedidos_CambiarEstado", cn
            );
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;
            cmd.Parameters.Add("@IdEstadoPedido", SqlDbType.Int).Value = idEstado;

            cn.Open();
            cmd.ExecuteNonQuery();

            return "Estado del pedido actualizado";
        }
    }
}
