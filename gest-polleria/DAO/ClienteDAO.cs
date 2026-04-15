using Microsoft.Data.SqlClient;
using gest_polleria.Models;
using System.Data;

namespace gest_polleria.DAO
{
    public class ClienteDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<Cliente> listar(bool? esEmpresa = null, string? buscar = null)
        {
            var lista = new List<Cliente>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Clientes_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@EsEmpresa", SqlDbType.Bit).Value = (object?)esEmpresa ?? DBNull.Value;
                    cmd.Parameters.Add("@Buscar", SqlDbType.NVarChar, 120).Value = (object?)buscar ?? DBNull.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente
                            {
                                IdCliente = dr.GetInt32(dr.GetOrdinal("IdCliente")),
                                TipoDocumento = dr["TipoDocumento"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("TipoDocumento")),
                                NumeroDocumento = dr["NumeroDocumento"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("NumeroDocumento")),
                                RazonSocial = dr["RazonSocial"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("RazonSocial")),
                                Nombres = dr["Nombres"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("Nombres")),
                                Apellidos = dr["Apellidos"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("Apellidos")),
                                Direccion = dr["Direccion"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("Direccion")),
                                Telefono = dr["Telefono"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("Telefono")),
                                Email = dr["Email"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("Email")),
                                EsEmpresa = dr.GetBoolean(dr.GetOrdinal("EsEmpresa"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public Cliente? buscar(int idCliente)
        {
            Cliente? c = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Clientes_Obtener", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            c = new Cliente
                            {
                                IdCliente = dr.GetInt32(dr.GetOrdinal("IdCliente")),
                                TipoDocumento = dr["TipoDocumento"] as string,
                                NumeroDocumento = dr["NumeroDocumento"] as string,
                                RazonSocial = dr["RazonSocial"] as string,
                                Nombres = dr["Nombres"] as string,
                                Apellidos = dr["Apellidos"] as string,
                                Direccion = dr["Direccion"] as string,
                                Telefono = dr["Telefono"] as string,
                                Email = dr["Email"] as string,
                                EsEmpresa = dr.GetBoolean(dr.GetOrdinal("EsEmpresa"))
                            };
                        }
                    }
                }
            }
            return c;
        }

        public string insertar(Cliente c, out int idClienteNuevo)
        {
            idClienteNuevo = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Clientes_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TipoDocumento", SqlDbType.NVarChar, 10).Value = (object?)c.TipoDocumento ?? DBNull.Value;
                    cmd.Parameters.Add("@NumeroDocumento", SqlDbType.NVarChar, 20).Value = (object?)c.NumeroDocumento ?? DBNull.Value;
                    cmd.Parameters.Add("@RazonSocial", SqlDbType.NVarChar, 120).Value = (object?)c.RazonSocial ?? DBNull.Value;
                    cmd.Parameters.Add("@Nombres", SqlDbType.NVarChar, 80).Value = (object?)c.Nombres ?? DBNull.Value;
                    cmd.Parameters.Add("@Apellidos", SqlDbType.NVarChar, 80).Value = (object?)c.Apellidos ?? DBNull.Value;
                    cmd.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = (object?)c.Direccion ?? DBNull.Value;
                    cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = (object?)c.Telefono ?? DBNull.Value;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)c.Email ?? DBNull.Value;
                    cmd.Parameters.Add("@EsEmpresa", SqlDbType.Bit).Value = c.EsEmpresa;

                    var pId = cmd.Parameters.Add("@IdClienteNuevo", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idClienteNuevo = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        public string actualizar(Cliente c)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Clientes_Actualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = c.IdCliente;
                    cmd.Parameters.Add("@TipoDocumento", SqlDbType.NVarChar, 10).Value = (object?)c.TipoDocumento ?? DBNull.Value;
                    cmd.Parameters.Add("@NumeroDocumento", SqlDbType.NVarChar, 20).Value = (object?)c.NumeroDocumento ?? DBNull.Value;
                    cmd.Parameters.Add("@RazonSocial", SqlDbType.NVarChar, 120).Value = (object?)c.RazonSocial ?? DBNull.Value;
                    cmd.Parameters.Add("@Nombres", SqlDbType.NVarChar, 80).Value = (object?)c.Nombres ?? DBNull.Value;
                    cmd.Parameters.Add("@Apellidos", SqlDbType.NVarChar, 80).Value = (object?)c.Apellidos ?? DBNull.Value;
                    cmd.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = (object?)c.Direccion ?? DBNull.Value;
                    cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = (object?)c.Telefono ?? DBNull.Value;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)c.Email ?? DBNull.Value;
                    cmd.Parameters.Add("@EsEmpresa", SqlDbType.Bit).Value = c.EsEmpresa;

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

        public string eliminar(int idCliente)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Clientes_Eliminar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;

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

        public string desactivar(int idCliente)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Clientes_Desactivar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;

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

        public string activar(int idCliente)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Clientes_Activar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;

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
