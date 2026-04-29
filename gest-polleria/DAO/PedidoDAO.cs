using Microsoft.Data.SqlClient;
using System.Data;
using gest_polleria.Models;

namespace gest_polleria.DAO
{
    public class PedidoDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<PedidoAbierto> listarPedidosAbiertos()
        {
            var lista = new List<PedidoAbierto>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT
                        p.IdPedido,
                        ISNULL(p.IdMesa, 0) AS IdMesa,
                        ISNULL(m.NumeroMesa, 0) AS NumeroMesa,
                        ISNULL(ep.Nombre, 'SIN ESTADO') AS EstadoPedido
                    FROM dbo.Pedidos p
                    LEFT JOIN dbo.Mesas m ON m.IdMesa = p.IdMesa
                    LEFT JOIN dbo.EstadosPedido ep ON ep.IdEstadoPedido = p.IdEstadoPedido
                    WHERE UPPER(ISNULL(ep.Nombre, '')) IN ('ABIERTO', 'REGISTRADO', 'PREPARACION', 'EN PREPARACION')
                    ORDER BY p.IdPedido DESC", cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new PedidoAbierto
                            {
                                IdPedido = dr.GetInt32(dr.GetOrdinal("IdPedido")),
                                IdMesa = dr.GetInt32(dr.GetOrdinal("IdMesa")),
                                NumeroMesa = dr.GetInt32(dr.GetOrdinal("NumeroMesa")),
                                EstadoPedido = dr.GetString(dr.GetOrdinal("EstadoPedido"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public IEnumerable<PedidoOperativo> listarPedidosOperativos()
        {
            var lista = new List<PedidoOperativo>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT
                        p.IdPedido,
                        CONVERT(VARCHAR(16), p.FechaHora, 120) AS FechaHora,
                        m.NumeroMesa,
                        ISNULL(tp.Nombre, 'SIN TIPO') AS TipoPedido,
                        ISNULL(ep.Nombre, 'SIN ESTADO') AS EstadoPedido,
                        ISNULL((
                            SELECT SUM(CAST(pd.Cantidad AS DECIMAL(18,2)) * pr.PrecioVenta)
                            FROM dbo.PedidosDetalle pd
                            INNER JOIN dbo.Productos pr ON pr.IdProducto = pd.IdProducto
                            WHERE pd.IdPedido = p.IdPedido
                              AND pd.Activo = 1
                        ), 0) AS TotalEstimado,
                        ISNULL(u.NombreCompleto, '') AS Mesero
                    FROM dbo.Pedidos p
                    LEFT JOIN dbo.Mesas m ON m.IdMesa = p.IdMesa
                    LEFT JOIN dbo.TiposPedido tp ON tp.IdTipoPedido = p.IdTipoPedido
                    LEFT JOIN dbo.EstadosPedido ep ON ep.IdEstadoPedido = p.IdEstadoPedido
                    LEFT JOIN dbo.Usuarios u ON u.IdUsuario = p.IdMesero
                    ORDER BY p.IdPedido DESC", cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new PedidoOperativo
                            {
                                IdPedido = dr.GetInt32(dr.GetOrdinal("IdPedido")),
                                FechaHora = dr.GetString(dr.GetOrdinal("FechaHora")),
                                NumeroMesa = dr["NumeroMesa"] == DBNull.Value ? null : dr.GetInt32(dr.GetOrdinal("NumeroMesa")),
                                TipoPedido = dr.GetString(dr.GetOrdinal("TipoPedido")),
                                EstadoPedido = dr.GetString(dr.GetOrdinal("EstadoPedido")),
                                TotalEstimado = dr.GetDecimal(dr.GetOrdinal("TotalEstimado")),
                                Mesero = dr.GetString(dr.GetOrdinal("Mesero"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public IEnumerable<PedidoDetalleLinea> listarDetallePedido(int idPedido)
        {
            var lista = new List<PedidoDetalleLinea>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT
                        pd.IdPedidoDetalle,
                        pd.IdPedido,
                        pd.IdProducto,
                        pr.Nombre AS Producto,
                        pd.Cantidad,
                        ISNULL(pd.Observacion, '') AS Observacion,
                        pr.PrecioVenta AS PrecioUnitario,
                        CAST(pd.Cantidad AS DECIMAL(18,2)) * pr.PrecioVenta AS Subtotal
                    FROM dbo.PedidosDetalle pd
                    INNER JOIN dbo.Productos pr ON pr.IdProducto = pd.IdProducto
                    WHERE pd.IdPedido = @IdPedido
                      AND pd.Activo = 1
                    ORDER BY pd.IdPedidoDetalle", cn))
                {
                    cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new PedidoDetalleLinea
                            {
                                IdPedidoDetalle = dr.GetInt32(dr.GetOrdinal("IdPedidoDetalle")),
                                IdPedido = dr.GetInt32(dr.GetOrdinal("IdPedido")),
                                IdProducto = dr.GetInt32(dr.GetOrdinal("IdProducto")),
                                Producto = dr.GetString(dr.GetOrdinal("Producto")),
                                Cantidad = dr.GetDecimal(dr.GetOrdinal("Cantidad")),
                                Observacion = dr.GetString(dr.GetOrdinal("Observacion")),
                                PrecioUnitario = dr.GetDecimal(dr.GetOrdinal("PrecioUnitario")),
                                Subtotal = dr.GetDecimal(dr.GetOrdinal("Subtotal"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public IEnumerable<ProductoCarta> listarCarta()
        {
            var lista = new List<ProductoCarta>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT
                        p.IdProducto,
                        p.Nombre,
                        ISNULL(c.Nombre, 'GENERAL') AS Categoria,
                        p.PrecioVenta,
                        p.ParaDelivery
                    FROM dbo.Productos p
                    LEFT JOIN dbo.CategoriasProducto c ON c.IdCategoria = p.IdCategoria
                    WHERE p.Activo = 1
                    ORDER BY c.Nombre, p.Nombre", cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ProductoCarta
                            {
                                IdProducto = dr.GetInt32(dr.GetOrdinal("IdProducto")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                Categoria = dr.GetString(dr.GetOrdinal("Categoria")),
                                PrecioVenta = dr.GetDecimal(dr.GetOrdinal("PrecioVenta")),
                                ParaDelivery = dr.GetBoolean(dr.GetOrdinal("ParaDelivery"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public string abrirMesa(int idMesa, int idTipoPedido, int? idMesero, out int idPedidoNuevo)
        {
            idPedidoNuevo = 0;
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Pedidos_AbrirMesa", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = idMesa;
                    cmd.Parameters.Add("@IdTipoPedido", SqlDbType.Int).Value = idTipoPedido;
                    cmd.Parameters.Add("@IdMesero", SqlDbType.Int).Value = (object?)idMesero ?? DBNull.Value;

                    var pId = cmd.Parameters.Add("@IdPedidoNuevo", SqlDbType.Int);
                    pId.Direction = ParameterDirection.Output;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && pId.Value != DBNull.Value)
                    {
                        idPedidoNuevo = Convert.ToInt32(pId.Value);

                        // Garantiza el estado operativo de la mesa aunque el SP de pedidos no lo actualice.
                        cambiarEstadoMesa(cn, idMesa, "OCUPADA");
                    }
                }
            }

            return mensaje;
        }

        public string enviarACocina(int idPedido)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Pedidos_EnviarCocina", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;

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

        public string cerrarMesa(int idPedido)
        {
            string mensaje = "No se recibió respuesta";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                int? idMesa = null;
                using (SqlCommand cmdMesa = new SqlCommand("SELECT TOP 1 IdMesa FROM dbo.Pedidos WHERE IdPedido = @IdPedido", cn))
                {
                    cmdMesa.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;
                    object valorMesa = cmdMesa.ExecuteScalar();
                    if (valorMesa != null && valorMesa != DBNull.Value)
                    {
                        idMesa = Convert.ToInt32(valorMesa);
                    }
                }

                using (SqlCommand cmd = new SqlCommand("dbo.usp_Pedidos_CerrarMesa", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    bool ok = pOk.Value != DBNull.Value && Convert.ToBoolean(pOk.Value);
                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;

                    if (ok && idMesa.HasValue)
                    {
                        cambiarEstadoMesa(cn, idMesa.Value, "LIBRE");
                    }
                }
            }
            return mensaje;
        }

        private static void cambiarEstadoMesa(SqlConnection cn, int idMesa, string estado)
        {
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
            }
        }

        public string registrarPedido(
     int idTipoPedido,
     int? idCliente,
     string? direccion,
     out int idPedido
 )
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand("usp_Pedidos_Registrar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdTipoPedido", SqlDbType.Int).Value = idTipoPedido;
            cmd.Parameters.Add("@IdEstadoPedido", SqlDbType.Int).Value = 1; // REGISTRADO
            cmd.Parameters.Add("@IdMesa", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = (object?)idCliente ?? DBNull.Value;
            cmd.Parameters.Add("@DireccionDelivery", SqlDbType.NVarChar, 200)
                          .Value = (object?)direccion ?? DBNull.Value;
            cmd.Parameters.Add("@TotalEstimado", SqlDbType.Decimal).Value = 0;

            var pId = cmd.Parameters.Add("@IdPedido", SqlDbType.Int);
            pId.Direction = ParameterDirection.Output;

            cn.Open();
            cmd.ExecuteNonQuery();

            idPedido = (int)pId.Value;
            return "Pedido registrado correctamente";
        }

        public string cambiarEstado(int idPedido, int idEstado)
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand(
                "usp_Pedidos_CambiarEstado", cn
            );
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = idPedido;
            cmd.Parameters.Add("@IdEstadoPedido", SqlDbType.Int).Value = idEstado;

            cn.Open();
            cmd.ExecuteNonQuery();

            return "Estado del pedido actualizado";
        }
    }
}
