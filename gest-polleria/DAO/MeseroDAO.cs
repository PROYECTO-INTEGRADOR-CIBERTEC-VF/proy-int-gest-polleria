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

        public List<Mesero> ListarMeseros(string nombre = "")
        {
            var lista = new List<Mesero>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ListarMeseros", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Mesero
                            {
                                IdMesero = dr.GetInt32(dr.GetOrdinal("IdMesero")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                Apellido = dr.GetString(dr.GetOrdinal("Apellido")),
                                DNI = dr.GetString(dr.GetOrdinal("DNI")),
                                Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? "" : dr.GetString(dr.GetOrdinal("Telefono")),
                                Turno = dr.GetString(dr.GetOrdinal("Turno")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo"))
                            });
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(nombre))
            {
                lista = lista.Where(m => (m.Nombre + " " + m.Apellido).Contains(nombre, StringComparison.OrdinalIgnoreCase) ||
                                          m.DNI.Contains(nombre)).ToList();
            }
            return lista;
        }

        public Mesero BuscarPorId(int id)
        {
            return ListarMeseros().FirstOrDefault(x => x.IdMesero == id);
        }

        // 3. REGISTRAR (Ajustado para capturar el mensaje de SQL)
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

                    // Usamos ExecuteScalar por si el SP devuelve un SELECT con el mensaje
                    var res = cmd.ExecuteScalar();
                    mensaje = res?.ToString() ?? "Mesero registrado correctamente.";
                }
                catch (SqlException ex) { mensaje = "Error: " + ex.Message; }
            }
            return mensaje;
        }

        // 4. ACTUALIZAR (Ya estaba bien, se mantiene igual)
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
                    var result = cmd.ExecuteScalar();
                    mensaje = result?.ToString() ?? "Actualización procesada.";
                }
                catch (SqlException ex) { mensaje = "Error: " + ex.Message; }
            }
            return mensaje;
        }

        // 5. CAMBIAR ESTADO
        public string CambiarEstado(int id)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_CambiarEstadoMesero", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMesero", id);

                    cn.Open();

                    var res = cmd.ExecuteScalar();
                    mensaje = res?.ToString() ?? "Estado actualizado correctamente.";
                }
                catch (SqlException ex)
                {
                    mensaje = "Error de base de datos: " + ex.Message;
                }
                catch (Exception ex)
                {
                    mensaje = "Error inesperado: " + ex.Message;
                }
            }
            return mensaje;
        }

        // 6. ASIGNAR MESA
        public string AsignarMeseroMesa(int idMesero, int idMesa)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_AsignarMeseroMesa", cn); // Llama al SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMesero", idMesero);
                    cmd.Parameters.AddWithValue("@IdMesa", idMesa);

                    cn.Open();
                    // ExecuteScalar atrapa el SELECT 'Mensaje...' que pusimos en el SQL
                    var res = cmd.ExecuteScalar();
                    mensaje = res?.ToString() ?? "Operación realizada.";
                }
                catch (SqlException ex)
                {
                    mensaje = "Error en base de datos: " + ex.Message;
                }
            }
            return mensaje;
        }

        // 7. LISTAR MESAS DISPONIBLES
        public List<Mesa> ListarMesasDisponibles()
        {
            var lista = new List<Mesa>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                   
                    string sql = "SELECT IdMesa, NumeroMesa, Descripcion, Capacidad FROM Mesas WHERE Activa = 1 AND IdMesero IS NULL";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Mesa
                            {
                                IdMesa = dr.GetInt32(dr.GetOrdinal("IdMesa")),
                                NumeroMesa = dr.GetInt32(dr.GetOrdinal("NumeroMesa")),
                                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? "" : dr.GetString(dr.GetOrdinal("Descripcion")),
                                Capacidad = dr.GetInt32(dr.GetOrdinal("Capacidad"))
                            });
                        }
                    }
                }
                catch (Exception) { /* Manejar error según necesites */ }
            }
            return lista;
        }
    }
}