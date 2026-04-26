using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class MeseroDAO
    {
        private readonly string cadena = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build()
            .GetConnectionString("cn") ?? "";

        // 1. MÉTODO LISTAR (FALTABA ESTO)
        public IEnumerable<Mesero> listar(string filtro = "", string turno = "")
        {
            var lista = new List<Mesero>();
            filtro = (filtro ?? "").Trim().ToLower();
            turno = (turno ?? "").Trim().ToLower();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_ListarMeseros", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var m = new Mesero
                            {
                                IdMesero = dr.GetInt32(dr.GetOrdinal("IdMesero")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")).Trim(),
                                Apellido = dr.GetString(dr.GetOrdinal("Apellido")).Trim(),
                                DNI = dr.GetString(dr.GetOrdinal("DNI")).Trim(),
                                Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? "" : dr.GetString(dr.GetOrdinal("Telefono")).Trim(),
                                Turno = dr.GetString(dr.GetOrdinal("Turno")).Trim(),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo"))
                            };

                            string nombreCompleto = (m.Nombre + " " + m.Apellido).ToLower();
                            bool coincideTexto = string.IsNullOrEmpty(filtro) || nombreCompleto.Contains(filtro) || m.DNI.Contains(filtro);
                            bool coincideTurno = string.IsNullOrEmpty(turno) || m.Turno.ToLower() == turno;

                            if (coincideTexto && coincideTurno) lista.Add(m);
                        }
                    }
                }
            }
            return lista;
        }

        // 2. MÉTODO BUSCAR (FALTABA ESTO)
        public Mesero? buscar(int idMesero)
        {
            Mesero? m = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_BuscarMeseroPorId", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdMesero", SqlDbType.Int).Value = idMesero;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            m = new Mesero
                            {
                                IdMesero = dr.GetInt32(dr.GetOrdinal("IdMesero")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                Apellido = dr.GetString(dr.GetOrdinal("Apellido")),
                                DNI = dr.GetString(dr.GetOrdinal("DNI")),
                                Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? "" : dr.GetString(dr.GetOrdinal("Telefono")),
                                Turno = dr.GetString(dr.GetOrdinal("Turno")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo"))
                            };
                        }
                    }
                }
            }
            return m;
        }

        // 3. MÉTODO REGISTRAR (FALTABA ESTO)

        public bool registrar(Mesero m)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_RegistrarMesero", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = m.Nombre;
                        cmd.Parameters.Add("@Apellido", SqlDbType.NVarChar, 50).Value = m.Apellido;
                        cmd.Parameters.Add("@DNI", SqlDbType.Char, 8).Value = m.DNI;
                        cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 15).Value = m.Telefono ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@Turno", SqlDbType.NVarChar, 20).Value = m.Turno;
                        cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = m.Activo;
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch { return false; }
        }

        // 4. MÉTODO ACTUALIZAR (FALTABA ESTO)
        public string actualizarConMensaje(Mesero m)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_ActualizarMesero", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdMesero", SqlDbType.Int).Value = m.IdMesero;
                        cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = m.Nombre;
                        cmd.Parameters.Add("@Apellido", SqlDbType.NVarChar, 50).Value = m.Apellido;
                        cmd.Parameters.Add("@DNI", SqlDbType.Char, 8).Value = m.DNI;
                        cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 15).Value = m.Telefono ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@Turno", SqlDbType.NVarChar, 20).Value = m.Turno;
                        cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = m.Activo;

                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "Operación completada";
                    }
                }
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        // 5. CAMBIAR ESTADO UNIFICADO (EL QUE YA TENÍAS)
        public string cambiarEstadoUnificado(int id)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_CambiarEstadoMesero", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdMesero", SqlDbType.Int).Value = id;
                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "Estado actualizado correctamente";
                    }
                }
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        // 6. ASIGNAR MESA (Para completar la funcionalidad de zonas/mesas)
        public string asignarMesa(int idMesero, int idMesa)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_AsignarMeseroMesa", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdMesero", SqlDbType.Int).Value = idMesero;
                        cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = idMesa;

                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "Mesa asignada correctamente";
                    }
                }
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        // 7. LISTAR MESAS DISPONIBLES (Para llenar el ComboBox)
        public List<Mesa> listarMesasDisponibles()
        {
            var lista = new List<Mesa>();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    string sql = "SELECT IdMesa, NumeroMesa, Descripcion, Capacidad FROM dbo.Mesas WHERE Activa = 1 AND IdMesero IS NULL";

                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
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
                }
            }
            catch (Exception)
            {
                return new List<Mesa>();
            }
            return lista;
        }
    }
}