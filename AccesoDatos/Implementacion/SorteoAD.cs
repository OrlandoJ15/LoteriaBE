using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Entidades.Models;
using MetodosComunes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Implementacion
{
    public class SorteoAD : ISorteoAD
    {

        public Excepciones gObjExcepciones = new Excepciones();

        public SqlCommandAbirCerrar gObjSqlCommandAbrirCerrar = new SqlCommandAbirCerrar();

        private readonly IConfiguration _configuration;

        public SorteoAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public List<Sorteo> RecSorteo()
        {
            List<Sorteo> lObjRespuesta = new List<Sorteo>();
            try
            {
                using (LoteriaContext lObjCnn = new LoteriaContext(_configuration))
                {

                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lObjCnn, "RecSorteoPA");
                    var lReader = lCmd.ExecuteReader();
                    while (lReader.Read())
                    {
                        Sorteo lobjDatosSorteo = new Sorteo();
                        lobjDatosSorteo.IdSorteo = Convert.ToInt32(lReader["idSorteo"]);
                        lobjDatosSorteo.Nombre = lReader["nombre"]?.ToString() ?? string.Empty;
                        lobjDatosSorteo.Numero = Convert.ToInt32(lReader["numero"]);
                        lobjDatosSorteo.Monto = Convert.ToInt32(lReader["monto"]);
                        lobjDatosSorteo.IdUsuario = Convert.ToInt32(lReader["idUsuario"]);
                        lobjDatosSorteo.IdTipoSorteo = Convert.ToInt32(lReader["idTipoSorteo"]);
                        lObjRespuesta.Add(lobjDatosSorteo);

                    }
                    gObjSqlCommandAbrirCerrar.CerrarConexion(lCmd);
                }
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
            return lObjRespuesta;
        }


        public Sorteo? RecSorteoXId(int pIdSorteo)
        {
            Sorteo? lObjRespuesta = null;  // Inicializamos como null
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecSorteoXIdPA");
                    lCmd.Parameters.Add(new SqlParameter("@idSorteo", pIdSorteo));
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto Sorteo
                    if (lReader.Read())
                    {
                        lObjRespuesta = new Sorteo
                        {
                            IdSorteo = Convert.ToInt32(lReader["idSorteo"]),
                            Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                            Numero = Convert.ToInt32(lReader["numero"]),
                            Monto = Convert.ToInt32(lReader["monto"]),
                            IdUsuario = Convert.ToInt32(lReader["idUsuario"]),
                            IdTipoSorteo = Convert.ToInt32(lReader["idTipoSorteo"])

                        };
                    }
                    gObjSqlCommandAbrirCerrar.CerrarConexion(lCmd);
                }
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                // Lanza la excepción para que la maneje la capa superior
                throw;
            }

            // Si no se encontró ningún registro, lObjRespuesta seguirá siendo null
            return lObjRespuesta;
        }

        public bool InsSorteo(Sorteo pSorteo)
        {
            return EjecutarProcedimiento("insSorteoPA", pSorteo);
        }

        public bool ModSorteo(Sorteo pSorteo)
        {
            return EjecutarProcedimiento("modSorteoPA", pSorteo);
        }

        public bool DelSorteo(Sorteo pSorteo)
        {
            return EjecutarProcedimiento("delSorteoPA", pSorteo);
        }

        // Método auxiliar para insertar, modificar o eliminar usuario
        private bool EjecutarProcedimiento(string procedimientoAlmacenado, Sorteo pSorteo)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, procedimientoAlmacenado);

                    if (procedimientoAlmacenado == "delSorteoPA")
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idSorteo", pSorteo.IdSorteo));
                    }
                    else
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idSorteo", pSorteo.IdSorteo));
                        lCmd.Parameters.Add(new SqlParameter("@nombre", pSorteo.Nombre));
                        lCmd.Parameters.Add(new SqlParameter("@numero", pSorteo.Numero));
                        lCmd.Parameters.Add(new SqlParameter("@monto", pSorteo.Monto));
                        lCmd.Parameters.Add(new SqlParameter("@idUsuario", pSorteo.IdUsuario));
                        lCmd.Parameters.Add(new SqlParameter("@idTipoSorteo", pSorteo.IdTipoSorteo));

                    }

                    if (lCmd.ExecuteNonQuery() > 0)
                    {
                        lObjRespuesta = true;
                    }

                    gObjSqlCommandAbrirCerrar.CerrarConexion(lCmd);
                }
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                throw;
            }
            return lObjRespuesta;
        }

    }
}
