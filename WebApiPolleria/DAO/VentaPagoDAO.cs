using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class VentaPagoDAO
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build())
            .GetConnectionString("cn") ?? "";

        public string registrarPago(VentaPago p)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_VentasPagos_Registrar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = p.IdVenta;
                    cmd.Parameters.Add("@IdMedioPago", SqlDbType.Int).Value = p.IdMedioPago;
                    cmd.Parameters.Add("@Monto", SqlDbType.Decimal).Value = p.Monto;

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
    }
}
