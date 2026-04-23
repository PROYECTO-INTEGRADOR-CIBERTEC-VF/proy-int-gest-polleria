using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class AlertaStockDAO
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build())
            .GetConnectionString("cn") ?? "";

        public IEnumerable<AlertaStock> listar()
        {
            var lista = new List<AlertaStock>();

            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand(
                "usp_Insumos_StockMinimo", cn
            );
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new AlertaStock
                {
                    IdInsumo = (int)dr["IdInsumo"],
                    Insumo = dr["Nombre"].ToString()!,
                    StockActual = (decimal)dr["StockActual"],
                    StockMinimo = (decimal)dr["StockMinimo"]
                });

            }

            return lista;
        }
    }
}
