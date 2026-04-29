using gest_polleria.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;

namespace gest_polleria.DAO
{
    public class MeseroDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<Mesero> listar()
        {
            var lista = new List<Mesero>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Meseros_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Mesero
                            {
                                IdUsuario = dr.GetInt32(dr.GetOrdinal("IdUsuario")),
                                UserName = dr.GetString(dr.GetOrdinal("UserName")),
                                NombreCompleto = dr.GetString(dr.GetOrdinal("NombreCompleto")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo")),
                                Rol = dr.GetString(dr.GetOrdinal("Rol")),
                                Turno = dr["Turno"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("Turno")),
                                Zona = dr["Zona"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("Zona"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public string registrar(
            string userName,
            string claveHash,
            string nombreCompleto,
            string? email,
            string? telefono,
            out int idUsuarioNuevo)
        {
            idUsuarioNuevo = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Meseros_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;
                    cmd.Parameters.Add("@ClaveHash", SqlDbType.NVarChar, 200).Value = claveHash;
                    cmd.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 100).Value = nombreCompleto;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)email ?? DBNull.Value;
                    cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = (object?)telefono ?? DBNull.Value;

                    var pId = cmd.Parameters.Add("@IdUsuarioNuevo", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idUsuarioNuevo = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        public string registrarTurno(MeseroTurno turno, out int idTurnoNuevo)
        {
            idTurnoNuevo = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Meseros_RegistrarTurno", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = turno.IdUsuario;
                    cmd.Parameters.Add("@Turno", SqlDbType.NVarChar, 50).Value = turno.Turno;
                    cmd.Parameters.Add("@HoraInicio", SqlDbType.Time).Value = turno.HoraInicio;
                    cmd.Parameters.Add("@HoraFin", SqlDbType.Time).Value = turno.HoraFin;

                    var pId = cmd.Parameters.Add("@IdMeseroTurnoNuevo", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idTurnoNuevo = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        public string asignarZona(int idUsuario, int idZona, out int idAsignacionNueva)
        {
            idAsignacionNueva = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Meseros_AsignarZona", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    cmd.Parameters.Add("@IdZona", SqlDbType.Int).Value = idZona;

                    var pId = cmd.Parameters.Add("@IdMeseroZonaNueva", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idAsignacionNueva = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        public IEnumerable<MeseroTurno> listarTurnos(int? idUsuario = null)
        {
            var lista = new List<MeseroTurno>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Meseros_ListarTurnos", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = (object?)idUsuario ?? DBNull.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new MeseroTurno
                            {
                                IdMeseroTurno = dr.GetInt32(dr.GetOrdinal("IdMeseroTurno")),
                                IdUsuario = dr.GetInt32(dr.GetOrdinal("IdUsuario")),
                                Turno = dr.GetString(dr.GetOrdinal("Turno")),
                                HoraInicio = dr.GetTimeSpan(dr.GetOrdinal("HoraInicio")),
                                HoraFin = dr.GetTimeSpan(dr.GetOrdinal("HoraFin")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public IEnumerable<MeseroZona> listarZonasAsignadas(int? idUsuario = null)
        {
            var lista = new List<MeseroZona>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Meseros_ListarZonas", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = (object?)idUsuario ?? DBNull.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new MeseroZona
                            {
                                IdMeseroZona = dr.GetInt32(dr.GetOrdinal("IdMeseroZona")),
                                IdUsuario = dr.GetInt32(dr.GetOrdinal("IdUsuario")),
                                IdZona = dr.GetInt32(dr.GetOrdinal("IdZona")),
                                Zona = dr.GetString(dr.GetOrdinal("Zona")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo"))
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}
