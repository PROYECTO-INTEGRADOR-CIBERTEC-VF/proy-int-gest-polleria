using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class UsuarioRolDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public string asignarRol(int idUsuario, int idRol)
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand("usp_UsuariosRoles_Asignar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
            cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;

            cmd.Parameters.Add("@Ok", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;

            cn.Open();
            cmd.ExecuteNonQuery();

            return cmd.Parameters["@Mensaje"].Value.ToString()!;
        }

        public string quitarRol(int idUsuario, int idRol)
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand("usp_UsuariosRoles_Quitar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
            cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;

            cmd.Parameters.Add("@Ok", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;

            cn.Open();
            cmd.ExecuteNonQuery();

            return cmd.Parameters["@Mensaje"].Value.ToString()!;
        }

        public IEnumerable<UsuarioRol> listarPorUsuario(int idUsuario)
        {
            var lista = new List<UsuarioRol>();

            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand(
                "usp_UsuariosRoles_ListarPorUsuario", cn
            );
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new UsuarioRol
                {
                    IdUsuario = (int)dr["IdUsuario"],
                    IdRol = (int)dr["IdRol"]
                });
            }

            return lista;
        }



    }
}
