using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class CatalogosDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public IEnumerable<Rol> listarRoles()
        {
            List<Rol> lista = new List<Rol>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_Roles_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Rol
                            {
                                IdRol = dr.GetInt32(dr.GetOrdinal("IdRol")),
                                NombreRol = dr.GetString(dr.GetOrdinal("NombreRol")),
                                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? null : dr.GetString(dr.GetOrdinal("Descripcion"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public IEnumerable<CategoriaProducto> listarCategorias()
        {
            List<CategoriaProducto> lista = new List<CategoriaProducto>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_CategoriasProducto_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new CategoriaProducto
                            {
                                IdCategoria = dr.GetInt32(dr.GetOrdinal("IdCategoria")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? null : dr.GetString(dr.GetOrdinal("Descripcion"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public IEnumerable<UnidadMedida> listarUnidades()
        {
            List<UnidadMedida> lista = new List<UnidadMedida>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_UnidadesMedida_Listar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new UnidadMedida
                            {
                                IdUnidadMedida = dr.GetInt32(dr.GetOrdinal("IdUnidadMedida")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                Abreviatura = dr.GetString(dr.GetOrdinal("Abreviatura"))
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}
