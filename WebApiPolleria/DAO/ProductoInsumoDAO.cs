using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class ProductoInsumoDAO
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build())
            .GetConnectionString("cn") ?? "";

        // RF-011: INSERTAR receta
        public string insertar(ProductoInsumo pi)
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand(
                "usp_ProductosInsumos_Insertar", cn
            );
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = pi.IdProducto;
            cmd.Parameters.Add("@IdInsumo", SqlDbType.Int).Value = pi.IdInsumo;
            cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = pi.Cantidad;

            var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
            pOk.Direction = ParameterDirection.Output;

            var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
            pMsg.Direction = ParameterDirection.Output;

            cn.Open();
            cmd.ExecuteNonQuery();

            return Convert.ToString(pMsg.Value) ?? "Sin mensaje";
        }

        // RF-011: ACTUALIZAR receta
        public string actualizar(ProductoInsumo pi)
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand(
                "usp_ProductosInsumos_Actualizar", cn
            );
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdProductoInsumo", SqlDbType.Int)
                .Value = pi.IdProductoInsumo;

            cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal)
                .Value = pi.Cantidad;

            var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
            pOk.Direction = ParameterDirection.Output;

            var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
            pMsg.Direction = ParameterDirection.Output;

            cn.Open();
            cmd.ExecuteNonQuery();

            return Convert.ToString(pMsg.Value) ?? "Sin mensaje";
        }

        // RF-011: ELIMINAR receta
        public string eliminar(int idProductoInsumo)
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand(
                "usp_ProductosInsumos_Eliminar", cn
            );
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdProductoInsumo", SqlDbType.Int)
                .Value = idProductoInsumo;

            var pOk = cmd.Parameters.Add("@Ok", SqlDbType.Bit);
            pOk.Direction = ParameterDirection.Output;

            var pMsg = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
            pMsg.Direction = ParameterDirection.Output;

            cn.Open();
            cmd.ExecuteNonQuery();

            return Convert.ToString(pMsg.Value) ?? "Sin mensaje";
        }

        // RF-011: LISTAR recetas por producto
        public IEnumerable<ProductoInsumo> listarPorProducto(int idProducto)
        {
            var lista = new List<ProductoInsumo>();

            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand(
                "usp_ProductosInsumos_ListarPorProducto", cn
            );
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new ProductoInsumo
                {
                    IdProductoInsumo = (int)dr["IdProductoInsumo"],
                    IdInsumo = (int)dr["IdInsumo"],
                    Insumo = dr["Insumo"].ToString(),
                    Cantidad = (decimal)dr["Cantidad"],
                    Abreviatura = dr["Abreviatura"].ToString()
                });
            }

            return lista;
        }
    }
}
