    using Microsoft.Data.SqlClient;
    using System.Data;
    using WebApiPolleria.Models;
using WebApiPolleria.Helpers;

namespace WebApiPolleria.DAO
    {
        public class UsuarioDAO
        {
            string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

            public IEnumerable<Usuario> listar(bool soloActivos = true, string? buscar = null)
            {
                var lista = new List<Usuario>();

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_Usuarios_Listar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SoloActivos", SqlDbType.Bit).Value = soloActivos;
                        cmd.Parameters.Add("@Buscar", SqlDbType.NVarChar, 100).Value = (object?)buscar ?? DBNull.Value;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Usuario
                                {
                                    IdUsuario = dr.GetInt32(dr.GetOrdinal("IdUsuario")),
                                    UserName = dr.GetString(dr.GetOrdinal("UserName")),
                                    NombreCompleto = dr.GetString(dr.GetOrdinal("NombreCompleto")),
                                    Email = dr.IsDBNull(dr.GetOrdinal("Email")) ? null : dr.GetString(dr.GetOrdinal("Email")),
                                    Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? null : dr.GetString(dr.GetOrdinal("Telefono")),
                                    Activo = dr.GetBoolean(dr.GetOrdinal("Activo")),
                                    FechaRegistro = dr.GetDateTime(dr.GetOrdinal("FechaRegistro"))
                                });
                            }
                        }
                    }
                }

                return lista;
            }

            public Usuario? buscar(int idUsuario)
            {
                Usuario? u = null;

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_Usuarios_Obtener", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                u = new Usuario
                                {
                                    IdUsuario = dr.GetInt32(dr.GetOrdinal("IdUsuario")),
                                    UserName = dr.GetString(dr.GetOrdinal("UserName")),
                                    ClaveHash = "NO_CARGAR",
                                    NombreCompleto = dr.GetString(dr.GetOrdinal("NombreCompleto")),
                                    Email = dr.IsDBNull(dr.GetOrdinal("Email")) ? null : dr.GetString(dr.GetOrdinal("Email")),
                                    Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? null : dr.GetString(dr.GetOrdinal("Telefono")),
                                    Activo = dr.GetBoolean(dr.GetOrdinal("Activo")),
                                    FechaRegistro = dr.GetDateTime(dr.GetOrdinal("FechaRegistro"))
                                };
                            }
                        }
                    }
                }

                return u;
            }

            public string insertar(Usuario u, out int idUsuarioNuevo)
            {
                idUsuarioNuevo = 0;
                string mensaje = "No se recibió respuesta";

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_Usuarios_Insertar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = u.UserName;
                        cmd.Parameters.Add("@ClaveHash", SqlDbType.NVarChar, 200).Value = u.ClaveHash;
                        cmd.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 100).Value = u.NombreCompleto;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)u.Email ?? DBNull.Value;
                        cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = (object?)u.Telefono ?? DBNull.Value;

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

            public string insertarDesdeRequest(UsuarioRequest req, out int idUsuarioNuevo)
            {
                var u = new Usuario
                {
                    UserName = req.UserName,
                    ClaveHash = PasswordHelper.Hash(req.Clave),
                    NombreCompleto = req.NombreCompleto,
                    Email = req.Email,
                    Telefono = req.Telefono,
                    Activo = true
                };

                return insertar(u, out idUsuarioNuevo);
            }


            public string actualizar(Usuario u)
            {
                string mensaje = "No se recibió respuesta";

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_Usuarios_Actualizar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = u.IdUsuario;
                        cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = u.UserName;
                        cmd.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 100).Value = u.NombreCompleto;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)u.Email ?? DBNull.Value;
                        cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = (object?)u.Telefono ?? DBNull.Value;
                        cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = u.Activo;

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

        public string actualizarDesdeRequest(UsuarioActualizarRequest req)
        {
            var u = new Usuario
            {
                IdUsuario = req.IdUsuario,
                UserName = req.UserName,
                NombreCompleto = req.NombreCompleto,
                Email = req.Email,
                Telefono = req.Telefono,
                Activo = req.Activo,

                // ClaveHash no se toca en esta actualización
                ClaveHash = "NO_USAR"
            };

            return actualizar(u);
        }


        public string desactivar(int idUsuario)
            {
                string mensaje = "No se recibió respuesta";

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_Usuarios_Desactivar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

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
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_Usuarios_Activar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = id;

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

        public string cambiarClave(int idUsuario, string clave, out bool ok)
        {
            ok = false;
            string mensaje = "No se recibió respuesta";
            string hash = PasswordHelper.Hash(clave);

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Usuarios_CambiarClave", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;


                    cmd.Parameters.Add("@ClaveHash", SqlDbType.NVarChar, 200).Value = hash;

                    var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
                    pOk.Direction = ParameterDirection.Output;

                    var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
                    pMsg.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    if (pOk.Value != DBNull.Value)
                        ok = Convert.ToBoolean(pOk.Value);

                    mensaje = Convert.ToString(pMsg.Value) ?? mensaje;
                }
            }

            return mensaje;
        }


    }
}

