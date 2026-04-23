using Microsoft.Data.SqlClient;
using System.Data;
using WebApiPolleria.Models;

namespace WebApiPolleria.DAO
{
    public class AjusteInventarioDAO
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public string registrar(AjusteInventario a)
        {
            using SqlConnection cn = new SqlConnection(cadena);
            using SqlCommand cmd = new SqlCommand(
                "usp_AjustesInventario_Registrar", cn
            );

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IdInsumo", SqlDbType.Int).Value = a.IdInsumo;
            cmd.Parameters.Add("@CantidadAjuste", SqlDbType.Decimal).Value = a.CantidadAjuste;
            cmd.Parameters.Add("@Motivo", SqlDbType.NVarChar, 200).Value = a.Motivo;
            cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = a.IdUsuario;

            cn.Open();
            cmd.ExecuteNonQuery();

            return "Ajuste de inventario registrado correctamente";
        }
    }
}
