using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class CompraDetalleDAO
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build())
            .GetConnectionString("cn") ?? "";

        public string agregarDetalle(CompraDetalle d)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_ComprasDetalle_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdCompra", SqlDbType.Int).Value = d.IdCompra;
                    cmd.Parameters.Add("@IdInsumo", SqlDbType.Int).Value = d.IdInsumo;
                    cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = d.Cantidad;
                    cmd.Parameters.Add("@CostoUnitario", SqlDbType.Decimal).Value = d.CostoUnitario;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    mensaje = Convert.ToString(pMsg.Value) ?? "";
                }
            }

            return mensaje;
        }
    }
}
