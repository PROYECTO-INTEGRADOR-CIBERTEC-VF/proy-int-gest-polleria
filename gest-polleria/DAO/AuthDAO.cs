using Microsoft.Data.SqlClient;
using gest_polleria.Models;
using System.Data;

namespace gest_polleria.DAO
{
    public class AuthDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public LoginResponse login(LoginRequest request)
        {
            var response = new LoginResponse();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Usuarios_Login", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = request.UserName;
                    cmd.Parameters.Add("@ClaveHash", SqlDbType.NVarChar, 256).Value = request.ClaveHash;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            response.IdUsuario = dr.GetInt32(dr.GetOrdinal("IdUsuario"));
                            response.UserName = dr.GetString(dr.GetOrdinal("UserName"));
                            response.NombreCompleto = dr.GetString(dr.GetOrdinal("NombreCompleto"));
                            response.Rol = dr.GetString(dr.GetOrdinal("Rol"));
                        }
                    }

                    response.Ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    response.Mensaje = Convert.ToString(pMsg.Value) ?? "Sin respuesta";
                }
            }

            return response;
        }
    }
}