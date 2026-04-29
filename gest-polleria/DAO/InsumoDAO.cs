using gest_polleria.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gest_polleria.DAO
{
    public class InsumoDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<Insumo> listar(bool soloActivos = true, string? buscar = null)
        {
            var lista = new List<Insumo>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Insumos_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SoloActivos", SqlDbType.Bit).Value = soloActivos;
                    cmd.Parameters.Add("@Buscar", SqlDbType.NVarChar, 120).Value = (object?)buscar ?? DBNull.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Insumo
                            {
                                IdInsumo = dr.GetInt32(dr.GetOrdinal("IdInsumo")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                IdUnidadMedida = dr.GetInt32(dr.GetOrdinal("IdUnidadMedida")),
                                StockMinimo = dr.GetDecimal(dr.GetOrdinal("StockMinimo")),
                                StockActual = dr.GetDecimal(dr.GetOrdinal("StockActual")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public Insumo? buscar(int idInsumo)
        {
            Insumo? insumo = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Insumos_Obtener", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdInsumo", SqlDbType.Int).Value = idInsumo;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            insumo = new Insumo
                            {
                                IdInsumo = dr.GetInt32(dr.GetOrdinal("IdInsumo")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                IdUnidadMedida = dr.GetInt32(dr.GetOrdinal("IdUnidadMedida")),
                                StockMinimo = dr.GetDecimal(dr.GetOrdinal("StockMinimo")),
                                StockActual = dr.GetDecimal(dr.GetOrdinal("StockActual")),
                                Activo = dr.GetBoolean(dr.GetOrdinal("Activo"))
                            };
                        }
                    }
                }
            }

            return insumo;
        }

        public string insertar(Insumo i, out int idInsumoNuevo)
        {
            idInsumoNuevo = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Insumos_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 120).Value = i.Nombre;
                    cmd.Parameters.Add("@IdUnidadMedida", SqlDbType.Int).Value = i.IdUnidadMedida;
                    cmd.Parameters.Add("@StockMinimo", SqlDbType.Decimal).Value = i.StockMinimo;
                    cmd.Parameters.Add("@StockActual", SqlDbType.Decimal).Value = i.StockActual;

                    var pId = cmd.Parameters.Add("@IdInsumoNuevo", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idInsumoNuevo = Convert.ToInt32(pId.Value);
                }
            }

            return mensaje;
        }

        public string actualizar(Insumo i)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Insumos_Actualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdInsumo", SqlDbType.Int).Value = i.IdInsumo;
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 120).Value = i.Nombre;
                    cmd.Parameters.Add("@IdUnidadMedida", SqlDbType.Int).Value = i.IdUnidadMedida;
                    cmd.Parameters.Add("@StockMinimo", SqlDbType.Decimal).Value = i.StockMinimo;
                    cmd.Parameters.Add("@StockActual", SqlDbType.Decimal).Value = i.StockActual;

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

        public string desactivar(int idInsumo)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Insumos_Desactivar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdInsumo", SqlDbType.Int).Value = idInsumo;

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

        public string ajustarStock(AjusteInventario ajuste)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Insumos_AjustarStock", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdInsumo", SqlDbType.Int).Value = ajuste.IdInsumo;
                    cmd.Parameters.Add("@CantidadAjuste", SqlDbType.Decimal).Value = ajuste.CantidadAjuste;
                    cmd.Parameters.Add("@Motivo", SqlDbType.NVarChar, 300).Value = ajuste.Motivo;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = ajuste.IdUsuario;

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

        public IEnumerable<StockReporte> alertas(decimal porcentaje = 10)
        {
            var lista = new List<StockReporte>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Insumos_Alertas", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Porcentaje", SqlDbType.Decimal).Value = porcentaje;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new StockReporte
                            {
                                IdInsumo = dr.GetInt32(dr.GetOrdinal("IdInsumo")),
                                Insumo = dr.GetString(dr.GetOrdinal("Insumo")),
                                StockActual = dr.GetDecimal(dr.GetOrdinal("StockActual")),
                                StockMinimo = dr.GetDecimal(dr.GetOrdinal("StockMinimo")),
                                EstadoStock = dr.GetString(dr.GetOrdinal("EstadoStock"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public IEnumerable<StockReporte> reporteStock()
        {
            var lista = new List<StockReporte>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Insumos_ReporteStock", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new StockReporte
                            {
                                IdInsumo = dr.GetInt32(dr.GetOrdinal("IdInsumo")),
                                Insumo = dr.GetString(dr.GetOrdinal("Insumo")),
                                StockActual = dr.GetDecimal(dr.GetOrdinal("StockActual")),
                                StockMinimo = dr.GetDecimal(dr.GetOrdinal("StockMinimo")),
                                EstadoStock = dr.GetString(dr.GetOrdinal("EstadoStock"))
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}
