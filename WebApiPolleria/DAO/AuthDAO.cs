using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;
using WebApiPolleria.Helpers;

namespace WebApiPolleria.DAO
{
    public class AuthDAO
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build())
            .GetConnectionString("cn") ?? "";

        public LoginResponse login(LoginRequest req)
        {
            var resp = new LoginResponse
            {
                Ok = false,
                Mensaje = "Usuario o contraseña incorrectos"
            };

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Usuarios_Login", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = req.Usuario;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                            return resp;    

                        int idUsuario = dr.GetInt32(dr.GetOrdinal("IdUsuario"));
                        string userName = dr.GetString(dr.GetOrdinal("UserName"));
                        string hash = dr.GetString(dr.GetOrdinal("ClaveHash"));
                        string nombre = dr.GetString(dr.GetOrdinal("NombreCompleto"));

                        // 🔐 VERIFICACIÓN REAL
                        if (!PasswordHelper.Verify(req.Clave, hash))
                            return resp;

                        // ✔ Login correcto
                        resp.Ok = true;
                        resp.Mensaje = "Login correcto";
                        resp.IdUsuario = idUsuario;
                        resp.Usuario = userName;
                        resp.NombreCompleto = nombre;
                    }
                }
            }
            return resp;
        }
    }
}
