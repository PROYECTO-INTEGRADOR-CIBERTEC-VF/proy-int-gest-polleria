using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class MesaDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<Mesa> listar(bool soloActivas = true, string? buscar = null)
        {
            List<Mesa> lista = new List<Mesa>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Mesas_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SoloActivas", SqlDbType.Bit).Value = soloActivas;
                    cmd.Parameters.Add("@Buscar", SqlDbType.NVarChar, 50).Value = (object?)buscar ?? DBNull.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Mesa
                            {
                                IdMesa = dr.GetInt32(dr.GetOrdinal("IdMesa")),
                                NumeroMesa = dr.GetInt32(dr.GetOrdinal("NumeroMesa")),
                                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? null : dr.GetString(dr.GetOrdinal("Descripcion")),
                                Capacidad = dr.GetInt32(dr.GetOrdinal("Capacidad")),
                                Activa = dr.GetBoolean(dr.GetOrdinal("Activa"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public Mesa? buscar(int idMesa)
        {
            Mesa? m = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Mesas_Obtener", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = idMesa;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            m = new Mesa
                            {
                                IdMesa = dr.GetInt32(dr.GetOrdinal("IdMesa")),
                                NumeroMesa = dr.GetInt32(dr.GetOrdinal("NumeroMesa")),
                                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? null : dr.GetString(dr.GetOrdinal("Descripcion")),
                                Capacidad = dr.GetInt32(dr.GetOrdinal("Capacidad")),
                                Activa = dr.GetBoolean(dr.GetOrdinal("Activa"))
                            };
                        }
                    }
                }
            }

            return m;
        }

        public string insertar(Mesa m, out int idMesaNueva)
        {
            idMesaNueva = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Mesas_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NumeroMesa", SqlDbType.Int).Value = m.NumeroMesa;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 100).Value = (object?)m.Descripcion ?? DBNull.Value;
                    cmd.Parameters.Add("@Capacidad", SqlDbType.Int).Value = m.Capacidad;

                    var pId = cmd.Parameters.Add("@IdMesaNueva", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idMesaNueva = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        public string actualizar(Mesa m)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Mesas_Actualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = m.IdMesa;
                    cmd.Parameters.Add("@NumeroMesa", SqlDbType.Int).Value = m.NumeroMesa;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 100).Value = (object?)m.Descripcion ?? DBNull.Value;
                    cmd.Parameters.Add("@Capacidad", SqlDbType.Int).Value = m.Capacidad;
                    cmd.Parameters.Add("@Activa", SqlDbType.Bit).Value = m.Activa;

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

        public string desactivar(int idMesa)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Mesas_Desactivar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = idMesa;

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

        public string activar(int id)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Mesas_Activar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = id;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                            mensaje = dr.GetString(0);
                        else
                            mensaje = "No se recibió respuesta";
                    }
                }
            }
            return mensaje;
        }

    }
}
