using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class CajaDAO
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build())
            .GetConnectionString("cn") ?? "";

        // RF-028: Apertura de caja
        public string abrirCaja(Caja c, out int idCajaTurnoNueva)
        {
            idCajaTurnoNueva = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Caja_Abrir", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdUsuarioApertura", SqlDbType.Int)
                        .Value = c.IdUsuarioApertura;

                    cmd.Parameters.Add("@MontoApertura", SqlDbType.Decimal)
                        .Value = c.MontoApertura;

                    cmd.Parameters.Add("@Observacion", SqlDbType.NVarChar, 300)
                        .Value = (object?)c.Observacion ?? DBNull.Value;

                    var pId = cmd.Parameters.Add("@IdCajaTurnoNueva", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idCajaTurnoNueva = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        // (Preparado para RF-028 cierre de caja)
        public string cerrarCaja(Caja c)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Caja_Cerrar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdCajaTurno", SqlDbType.Int)
                        .Value = c.IdCajaTurno;

                    cmd.Parameters.Add("@IdUsuarioCierre", SqlDbType.Int)
                        .Value = c.IdUsuarioCierre;

                    cmd.Parameters.Add("@Observacion", SqlDbType.NVarChar, 300)
                        .Value = (object?)c.Observacion ?? DBNull.Value;

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
