using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class CompraDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public string registrarCompra(Compra c, out int idCompraNueva)
        {
            idCompraNueva = 0;
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Compras_Registrar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = c.IdProveedor;
                    cmd.Parameters.Add("@TipoComprobante", SqlDbType.NVarChar, 20).Value = c.TipoComprobante;
                    cmd.Parameters.Add("@Serie", SqlDbType.NVarChar, 10).Value = c.Serie;
                    cmd.Parameters.Add("@Numero", SqlDbType.NVarChar, 20).Value = c.Numero;

                    var pId = cmd.Parameters.Add("@IdCompraNueva", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    if (Convert.ToBoolean(pOk.Value))
                        idCompraNueva = Convert.ToInt32(pId.Value);

                    mensaje = Convert.ToString(pMsg.Value) ?? "";
                }
            }
            return mensaje;
        }
    }
}
