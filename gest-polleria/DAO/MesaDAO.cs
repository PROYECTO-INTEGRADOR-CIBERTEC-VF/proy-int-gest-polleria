using gest_polleria.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gest_polleria.DAO
{
    public class MesaDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<Mesa> listar(bool soloActivas = true)
        {
            var lista = new List<Mesa>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Mesas_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SoloActivas", SqlDbType.Bit).Value = soloActivas;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Mesa
                            {
                                IdMesa = dr.GetInt32(dr.GetOrdinal("IdMesa")),
                                NumeroMesa = dr.GetInt32(dr.GetOrdinal("NumeroMesa")),
                                Descripcion = dr["Descripcion"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("Descripcion")),
                                Capacidad = dr.GetInt32(dr.GetOrdinal("Capacidad")),
                                Activa = dr.GetBoolean(dr.GetOrdinal("Activa")),
                                Estado = dr.GetString(dr.GetOrdinal("Estado"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public string insertar(Mesa mesa, out int idMesaNueva)
        {
            idMesaNueva = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Mesas_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NumeroMesa", SqlDbType.Int).Value = mesa.NumeroMesa;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 100).Value = (object?)mesa.Descripcion ?? DBNull.Value;
                    cmd.Parameters.Add("@Capacidad", SqlDbType.Int).Value = mesa.Capacidad;

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

        public string actualizar(Mesa mesa)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Mesas_Actualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = mesa.IdMesa;
                    cmd.Parameters.Add("@NumeroMesa", SqlDbType.Int).Value = mesa.NumeroMesa;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 100).Value = (object?)mesa.Descripcion ?? DBNull.Value;
                    cmd.Parameters.Add("@Capacidad", SqlDbType.Int).Value = mesa.Capacidad;

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

        public string cambiarEstado(int idMesa, string estado)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Mesas_CambiarEstado", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = idMesa;
                    cmd.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = estado;

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
    }
}
