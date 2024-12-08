using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Entidades.Models;
using MetodosComunes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Implementacion
{
    public class TipoSorteoAD : ITipoSorteoAD
    {

        public Excepciones gObjExcepciones = new Excepciones();

        public SqlCommandAbirCerrar gObjSqlCommandAbrirCerrar = new SqlCommandAbirCerrar();

        private readonly IConfiguration _configuration;

        public TipoSorteoAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public List<TipoSorteo> RecTipoSorteo()
        {
            List<TipoSorteo> lObjRespuesta = new List<TipoSorteo>();
            try
            {
                using (LoteriaContext lObjCnn = new LoteriaContext(_configuration))
                {

                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lObjCnn, "RecTipoSorteoPA");
                    var lReader = lCmd.ExecuteReader();
                    while (lReader.Read())
                    {
                        TipoSorteo lobjDatosTipoSorteo = new TipoSorteo();
                        lobjDatosTipoSorteo.Id = Convert.ToInt32(lReader["id"]);
                        lobjDatosTipoSorteo.NumeroGanador = lReader["numeroGanador"] != DBNull.Value ? Convert.ToInt32(lReader["numeroGanador"]) : (int?)null;
                        lobjDatosTipoSorteo.IdTipoSorteoGeneral = Convert.ToInt32(lReader["idTipoSorteoGeneral"]);
                        lobjDatosTipoSorteo.Fecha = Convert.ToDateTime(lReader["fecha"]);
                        lobjDatosTipoSorteo.NombreTipoSorteoGeneral = lReader["nombreTipoSorteoGeneral"].ToString() ?? string.Empty;

                        lObjRespuesta.Add(lobjDatosTipoSorteo);

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


        public TipoSorteo? RecTipoSorteoXId(int pIdTipoSorteo)
        {
            TipoSorteo? lObjRespuesta = null;  // Inicializamos como null
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecTipoSorteoXIdPA");
                    lCmd.Parameters.Add(new SqlParameter("@id", pIdTipoSorteo));
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto TipoSorteo
                    if (lReader.Read())
                    {
                        lObjRespuesta = new TipoSorteo
                        {
                            Id = Convert.ToInt32(lReader["id"]),
                            NumeroGanador = Convert.ToInt32(lReader["numeroGanador"]),
                            IdTipoSorteoGeneral = Convert.ToInt32(lReader["idTipoSorteoGeneral"]),
                            Fecha = Convert.ToDateTime(lReader["fecha"]),
                            NombreTipoSorteoGeneral = lReader["nombreTipoSorteoGeneral"].ToString() ?? string.Empty,
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

        public bool InsTipoSorteo(TipoSorteo pTipoSorteo)
        {
            return EjecutarProcedimiento("insTipoSorteoPA", pTipoSorteo);
        }

        public bool ModTipoSorteo(TipoSorteo pTipoSorteo)
        {
            return EjecutarProcedimiento("modTipoSorteoPA", pTipoSorteo);
        }

        public bool DelTipoSorteo(TipoSorteo pTipoSorteo)
        {
            return EjecutarProcedimiento("delTipoSorteoPA", pTipoSorteo);
        }

        // Método auxiliar para insertar, modificar o eliminar usuario
        private bool EjecutarProcedimiento(string procedimientoAlmacenado, TipoSorteo pTipoSorteo)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, procedimientoAlmacenado);

                    if (procedimientoAlmacenado == "delTipoSorteoPA")
                    {
                        lCmd.Parameters.Add(new SqlParameter("@id", pTipoSorteo.Id));
                    }
                    else
                    {
                        lCmd.Parameters.Add(new SqlParameter("@id", pTipoSorteo.Id));
                        lCmd.Parameters.Add(new SqlParameter("@numeroGanador", pTipoSorteo.NumeroGanador));
                        lCmd.Parameters.Add(new SqlParameter("@idTipoSorteoGeneral", pTipoSorteo.IdTipoSorteoGeneral));
                        lCmd.Parameters.Add(new SqlParameter("@fecha", pTipoSorteo.Fecha));

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






        public int RecIdTipoSorteoFromTipoSorteoGeneralOExtraordinario(int pTipoSorteo)
        {
            int lObjRespuesta = 0;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecIdTipoSorteoFromTipoSorteoGeneralOExtraordinarioPA");
                    lCmd.Parameters.Add(new SqlParameter("@id", pTipoSorteo));


                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto TipoSorteo
                    if (lReader.Read())
                    {
                        lObjRespuesta = Convert.ToInt32(lReader["id"]);
                                               
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



    }
}
