using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class ProveedorDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<Proveedor> listar(bool soloActivos = true, string? buscar = null)
        {
            List<Proveedor> lista = new List<Proveedor>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Proveedores_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SoloActivos", SqlDbType.Bit).Value = soloActivos;
                    cmd.Parameters.Add("@Buscar", SqlDbType.NVarChar, 120).Value = (object?)buscar ?? DBNull.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Proveedor
                            {
                                IdProveedor = dr.GetInt32(dr.GetOrdinal("IdProveedor")),
                                Ruc = dr.GetString(dr.GetOrdinal("Ruc")),
                                RazonSocial = dr.GetString(dr.GetOrdinal("RazonSocial")),
                                Direccion = dr.IsDBNull(dr.GetOrdinal("Direccion")) ? null : dr.GetString(dr.GetOrdinal("Direccion")),
                                Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? null : dr.GetString(dr.GetOrdinal("Telefono")),
                                Email = dr.IsDBNull(dr.GetOrdinal("Email")) ? null : dr.GetString(dr.GetOrdinal("Email")),
                                Contacto = dr.IsDBNull(dr.GetOrdinal("Contacto")) ? null : dr.GetString(dr.GetOrdinal("Contacto")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo"))
                            });
                        }
                    }
                }
            }

            return lista;
        }
        public Proveedor? buscar(int idProveedor)
        {
            Proveedor? p = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Proveedores_Obtener", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = idProveedor;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            p = new Proveedor
                            {
                                IdProveedor = dr.GetInt32(dr.GetOrdinal("IdProveedor")),
                                Ruc = dr.GetString(dr.GetOrdinal("Ruc")),
                                RazonSocial = dr.GetString(dr.GetOrdinal("RazonSocial")),
                                Direccion = dr.IsDBNull(dr.GetOrdinal("Direccion")) ? null : dr.GetString(dr.GetOrdinal("Direccion")),
                                Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? null : dr.GetString(dr.GetOrdinal("Telefono")),
                                Email = dr.IsDBNull(dr.GetOrdinal("Email")) ? null : dr.GetString(dr.GetOrdinal("Email")),
                                Contacto = dr.IsDBNull(dr.GetOrdinal("Contacto")) ? null : dr.GetString(dr.GetOrdinal("Contacto")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo"))
                            };
                        }
                    }
                }
            }

            return p;
        }

        public string insertar(Proveedor p, out int idProveedorNuevo)
        {
            idProveedorNuevo = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Proveedores_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Ruc", SqlDbType.NVarChar, 20).Value = p.Ruc;
                    cmd.Parameters.Add("@RazonSocial", SqlDbType.NVarChar, 120).Value = p.RazonSocial;
                    cmd.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = (object?)p.Direccion ?? DBNull.Value;
                    cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = (object?)p.Telefono ?? DBNull.Value;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)p.Email ?? DBNull.Value;
                    cmd.Parameters.Add("@Contacto", SqlDbType.NVarChar, 80).Value = (object?)p.Contacto ?? DBNull.Value;

                    var pId = cmd.Parameters.Add("@IdProveedorNuevo", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idProveedorNuevo = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        public string actualizar(Proveedor p)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Proveedores_Actualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = p.IdProveedor;
                    cmd.Parameters.Add("@Ruc", SqlDbType.NVarChar, 20).Value = p.Ruc;
                    cmd.Parameters.Add("@RazonSocial", SqlDbType.NVarChar, 120).Value = p.RazonSocial;
                    cmd.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = (object?)p.Direccion ?? DBNull.Value;
                    cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = (object?)p.Telefono ?? DBNull.Value;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)p.Email ?? DBNull.Value;
                    cmd.Parameters.Add("@Contacto", SqlDbType.NVarChar, 80).Value = (object?)p.Contacto ?? DBNull.Value;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = p.Activo;

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

        public string desactivar(int idProveedor)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Proveedores_Desactivar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = idProveedor;

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
                using (SqlCommand cmd = new SqlCommand("usp_Proveedores_Activar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = id;

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
