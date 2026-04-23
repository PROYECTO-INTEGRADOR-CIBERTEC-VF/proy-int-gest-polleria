using Microsoft.Data.SqlClient;
using System.Data;
using gest_polleria.Models;

namespace gest_polleria.DAO
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
                        Estado = dr.GetBoolean(5),
                        Turno = dr.GetString(6)
                    });
                }
            }
            return lista;
        }


        //ASIGNAR MESERO A MESA/ZONA
        public string AsignarMeseroMesa(int idMesero, int idMesa)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    
                    SqlCommand cmd = new SqlCommand("usp_AsignarMeseroMesa", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idMesero", idMesero);
                    cmd.Parameters.AddWithValue("@idMesa", idMesa);

                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se asignó al mesero correctamente. Filas afectadas: {i}";
                }
                catch (SqlException ex)
                {
                    mensaje = "Error en la base de datos: " + ex.Message;
                }
            }
            return mensaje;
        }

        //REGISTRAR MESERO
        public string RegistrarMesero(Mesero reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarMesero", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Mapeo de parámetros desde el modelo Mesero 
                    cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", reg.Apellido);
                    cmd.Parameters.AddWithValue("@dni", reg.DNI);
                    cmd.Parameters.AddWithValue("@telefono", reg.Telefono);
                    cmd.Parameters.AddWithValue("@estado", reg.Estado);
                    cmd.Parameters.AddWithValue("@turno", reg.Turno);

                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"El mesero {reg.Nombre} {reg.Apellido} se registró con éxito.";
                }
                catch (SqlException ex)
                {
                    mensaje = "Error al registrar en la BD: " + ex.Message;
                }
            }
            return mensaje;
        }
    }
}
