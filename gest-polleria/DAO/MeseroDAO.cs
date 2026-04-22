using Microsoft.Data.SqlClient;
using System.Data;
using gest_polleria.Models;

namespace WebApiPolleria.DAO
{
    public class MeseroDAO
    {
        private readonly string cadena;

        public MeseroDAO(IConfiguration config)
        {
            cadena = config.GetConnectionString("cn");
        }

        public List<Mesero> ListarMeseros()
        {
            var lista = new List<Mesero>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("usp_ListarMeseros", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Mesero
                    {
                        IdMesero = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Apellido = dr.GetString(2),
                        DNI = dr.GetString(3),
                        Telefono = dr.GetString(4),
                        Estado = dr.GetBoolean(5)
                    });
                }
            }
            return lista;
        }
    }
}