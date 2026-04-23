using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class VentaDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build())
            .GetConnectionString("cn") ?? "";

        public IEnumerable<VentaConsultaDTO> consultar(
        string? serie = null,
         string? numero = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        int? idEstadoComprobante = null,
        int? idCliente = null)
        {
            var lista = new List<VentaConsultaDTO>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Ventas_Consultar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Serie", SqlDbType.NVarChar, 10).Value = (object?)serie ?? DBNull.Value;
                    cmd.Parameters.Add("@Numero", SqlDbType.NVarChar, 20).Value = (object?)numero ?? DBNull.Value;
                    cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = (object?)fechaInicio ?? DBNull.Value;
                    cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = (object?)fechaFin ?? DBNull.Value;
                    cmd.Parameters.Add("@IdEstadoComprobante", SqlDbType.Int).Value = (object?)idEstadoComprobante ?? DBNull.Value;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = (object?)idCliente ?? DBNull.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new VentaConsultaDTO
                            {
                                IdVenta = dr.GetInt32(dr.GetOrdinal("IdVenta")),
                                FechaHora = dr.GetDateTime(dr.GetOrdinal("FechaHora")),
                                Serie = dr.GetString(dr.GetOrdinal("Serie")),
                                Numero = dr.GetString(dr.GetOrdinal("Numero")),
                                Subtotal = dr.GetDecimal(dr.GetOrdinal("Subtotal")),
                                Igv = dr.GetDecimal(dr.GetOrdinal("Igv")),
                                Total = dr.GetDecimal(dr.GetOrdinal("Total")),
                                EstadoComprobante = dr.GetString(dr.GetOrdinal("EstadoComprobante")),
                                Cliente = dr["Cliente"] == DBNull.Value ? null : dr.GetString(dr.GetOrdinal("Cliente"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public string registrarVenta(Venta v, out int idVentaNueva)
        {
            idVentaNueva = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Ventas_Registrar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = v.IdPedido;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = (object?)v.IdCliente ?? DBNull.Value;
                    cmd.Parameters.Add("@IdTipoComprobante", SqlDbType.Int).Value = v.IdTipoComprobante;
                    cmd.Parameters.Add("@Serie", SqlDbType.NVarChar, 10).Value = v.Serie;
                    cmd.Parameters.Add("@Numero", SqlDbType.NVarChar, 20).Value = v.Numero;
                    cmd.Parameters.Add("@IdUsuarioCajero", SqlDbType.Int).Value = v.IdUsuarioCajero;
                    cmd.Parameters.Add("@IdCajaTurno", SqlDbType.Int).Value = v.IdCajaTurno;
                    cmd.Parameters.Add("@IdEstadoComprobante", SqlDbType.Int).Value = v.IdEstadoComprobante;

                    var pId = cmd.Parameters.Add("@IdVentaNueva", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                        idVentaNueva = Convert.ToInt32(pId.Value);
                }
            }
            return mensaje;
        }

        public string anularVenta(AnulacionVenta a)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Ventas_Anular", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = a.IdVenta;
                    cmd.Parameters.Add("@Motivo", SqlDbType.NVarChar, 300).Value = a.Motivo;

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

        public string generarElectronico(GeneracionElectronica g)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Ventas_GenerarElectronico", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = g.IdVenta;

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

        public VentaPdfModel? obtenerVentaParaPdf(int idVenta)
        {
            VentaPdfModel? venta = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Ventas_ObtenerParaPdf", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        /* ======================
                           1) CABECERA
                           ====================== */
                        if (!dr.Read())
                            return null;

                        venta = new VentaPdfModel
                        {
                            Serie = dr.GetString(dr.GetOrdinal("Serie")),
                            Numero = dr.GetString(dr.GetOrdinal("Numero")),
                            FechaHora = dr.GetDateTime(dr.GetOrdinal("FechaHora")),
                            Subtotal = dr.GetDecimal(dr.GetOrdinal("Subtotal")),
                            Igv = dr.GetDecimal(dr.GetOrdinal("Igv")),
                            Total = dr.GetDecimal(dr.GetOrdinal("Total"))
                        };

                        /* ======================
                           2) DETALLE
                           ====================== */
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                venta.Detalle.Add(new VentaPdfDetalle
                                {
                                    Producto = dr.GetString(dr.GetOrdinal("Producto")),
                                    Cantidad = dr.GetDecimal(dr.GetOrdinal("Cantidad")),
                                    PrecioUnitario = dr.GetDecimal(dr.GetOrdinal("PrecioUnitario")),
                                    Subtotal = dr.GetDecimal(dr.GetOrdinal("Subtotal"))
                                });
                            }
                        }
                    }
                }
            }

            return venta;
        }

        public string enviarSunat(int idVenta)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Ventas_EnviarSunat", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;

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

        public string recibirCdr(int idVenta)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Ventas_RecibirCdr", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;

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

        public (bool ok, string mensaje, string? rutaPdf) enviarCliente(int idVenta)
        {
            bool ok = false;
            string mensaje = "No se recibió respuesta";
            string? rutaPdf = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Ventas_EnviarCliente", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;

                    var pRuta = cmd.Parameters.Add("@RutaPdf", SqlDbType.NVarChar, 300);
                    pRuta.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;
                    rutaPdf = pRuta.Value == DBNull.Value ? null : Convert.ToString(pRuta.Value);
                }
            }

            return (ok, mensaje, rutaPdf);
        }






    }
}
