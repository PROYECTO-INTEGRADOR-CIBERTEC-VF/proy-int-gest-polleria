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
                        Telefono = dr.IsDBNull(4) ? "" : dr.GetString(4),
                        Turno = dr.GetString(5),
                        Activo = dr.GetBoolean(6)
                    });
                }
            }
            return lista;
        }

        public string RegistrarMesero(Mesero reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarMesero", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", reg.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", reg.Apellido);
                    cmd.Parameters.AddWithValue("@DNI", reg.DNI);
                    cmd.Parameters.AddWithValue("@Telefono", reg.Telefono ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Turno", reg.Turno);
                    cmd.Parameters.AddWithValue("@Activo", reg.Activo);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Mesero registrado correctamente.";
                }
                catch (SqlException ex) { mensaje = "Error: " + ex.Message; }
            }
            return mensaje;
        }

        public string ActualizarMesero(Mesero reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ActualizarMesero", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMesero", reg.IdMesero);
                    cmd.Parameters.AddWithValue("@Nombre", reg.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", reg.Apellido);
                    cmd.Parameters.AddWithValue("@DNI", reg.DNI);
                    cmd.Parameters.AddWithValue("@Telefono", reg.Telefono ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Turno", reg.Turno);
                    cmd.Parameters.AddWithValue("@Activo", reg.Activo);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Mesero actualizado correctamente.";
                }
                catch (SqlException ex) { mensaje = "Error: " + ex.Message; }
            }
            return mensaje;
        }

        public string CambiarEstado(int id, bool activar)
        {
            string sp = activar ? "usp_ActivarMesero" : "usp_DesactivarMesero";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand(sp, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMesero", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return activar ? "Activado" : "Desactivado";
        }

        public string AsignarMeseroMesa(int idMesero, int idMesa)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_AsignarMeseroMesa", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMesero", idMesero);
                    cmd.Parameters.AddWithValue("@IdMesa", idMesa);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Mesa asignada correctamente.";
                }
                catch (SqlException ex) { mensaje = "Error: " + ex.Message; }
            }
            return mensaje;
        }
    }
}