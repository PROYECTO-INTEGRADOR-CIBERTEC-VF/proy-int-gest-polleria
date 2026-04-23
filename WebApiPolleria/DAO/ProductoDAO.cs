using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class ProductoDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<Producto> listar(bool soloActivos = true, int? idCategoria = null, string? buscar = null)
        {
            var lista = new List<Producto>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Productos_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SoloActivos", SqlDbType.Bit).Value = soloActivos;
                    cmd.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = (object?)idCategoria ?? DBNull.Value;
                    cmd.Parameters.Add("@Buscar", SqlDbType.NVarChar, 120).Value = (object?)buscar ?? DBNull.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto
                            {
                                IdProducto = dr.GetInt32(dr.GetOrdinal("IdProducto")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                IdCategoria = dr.GetInt32(dr.GetOrdinal("IdCategoria")),
                                PrecioVenta = dr.GetDecimal(dr.GetOrdinal("PrecioVenta")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo")),
                                EsCombo = dr.GetBoolean(dr.GetOrdinal("EsCombo")),
                                ParaDelivery = dr.GetBoolean(dr.GetOrdinal("ParaDelivery"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public Producto? buscar(int idProducto)
        {
            Producto? p = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Productos_Obtener", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            p = new Producto
                            {
                                IdProducto = dr.GetInt32(dr.GetOrdinal("IdProducto")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                IdCategoria = dr.GetInt32(dr.GetOrdinal("IdCategoria")),
                                PrecioVenta = dr.GetDecimal(dr.GetOrdinal("PrecioVenta")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo")),
                                EsCombo = dr.GetBoolean(dr.GetOrdinal("EsCombo")),
                                ParaDelivery = dr.GetBoolean(dr.GetOrdinal("ParaDelivery"))
                            };
                        }
                    }
                }
            }

            return p;
        }

        public string insertar(Producto p, out int idProductoNuevo)
        {
            idProductoNuevo = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Productos_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 120).Value = p.Nombre;
                    cmd.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = p.IdCategoria;
                    cmd.Parameters.Add("@PrecioVenta", SqlDbType.Decimal).Value = p.PrecioVenta;
                    cmd.Parameters.Add("@EsCombo", SqlDbType.Bit).Value = p.EsCombo;
                    cmd.Parameters.Add("@ParaDelivery", SqlDbType.Bit).Value = p.ParaDelivery;

                    var pId = cmd.Parameters.Add("@IdProductoNuevo", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idProductoNuevo = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        public string actualizar(Producto p)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Productos_Actualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = p.IdProducto;
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 120).Value = p.Nombre;
                    cmd.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = p.IdCategoria;
                    cmd.Parameters.Add("@PrecioVenta", SqlDbType.Decimal).Value = p.PrecioVenta;
                    cmd.Parameters.Add("@EsCombo", SqlDbType.Bit).Value = p.EsCombo;
                    cmd.Parameters.Add("@ParaDelivery", SqlDbType.Bit).Value = p.ParaDelivery;
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

        public string desactivar(int idProducto)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Productos_Desactivar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;

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
                using (SqlCommand cmd = new SqlCommand("usp_Productos_Activar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = id;

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
