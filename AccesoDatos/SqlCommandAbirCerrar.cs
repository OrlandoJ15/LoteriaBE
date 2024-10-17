using AccesoDatos.DBContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos
{
    public class SqlCommandAbirCerrar
    {
       // private readonly IConfiguration _configuration;

       // public SqlCommandAbirCerrar (IConfiguration configuration)
        //{
          //  _configuration = configuration;
       // }

        public SqlCommand CrearComando(LoteriaContext lobjCnn, string procedimientoAlmacenado)
        {
            var lCmd = (SqlCommand)lobjCnn.Database.GetDbConnection().CreateCommand();
            lCmd.CommandText = procedimientoAlmacenado;
            lCmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
            {
                lCmd.Connection.Open();
            }
            return lCmd;
        }

        // Método auxiliar para cerrar la conexión
        public void CerrarConexion(SqlCommand lCmd)
        {
            if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
            {
                lCmd.Connection.Close();
            }
        }
    }
}
