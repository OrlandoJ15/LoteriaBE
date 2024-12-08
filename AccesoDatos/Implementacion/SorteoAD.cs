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
                        lobjDatosSorteo.Id = Convert.ToInt32(lReader["id"]);
                        lobjDatosSorteo.IdUsuario = Convert.ToInt32(lReader["idUsuario"]);
                        lobjDatosSorteo.IdTipoSorteo = Convert.ToInt32(lReader["idTipoSorteo"]);
                        lobjDatosSorteo.NombreUsuario = lReader["nombreUsuario"].ToString() ?? string.Empty;
                        lobjDatosSorteo.NombreTipoSorteoGeneral = lReader["nombreTipoSorteoGeneral"].ToString() ?? string.Empty;
                        lobjDatosSorteo.FechaTipoSorteo = Convert.ToDateTime(lReader["fechaTipoSorteo"]);

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
                    lCmd.Parameters.Add(new SqlParameter("@id", pIdSorteo));
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto Sorteo
                    if (lReader.Read())
                    {
                        lObjRespuesta = new Sorteo
                        {
                            Id = Convert.ToInt32(lReader["id"]),
                            IdUsuario = Convert.ToInt32(lReader["idUsuario"]),
                            IdTipoSorteo = Convert.ToInt32(lReader["idTipoSorteo"]),
                            NombreUsuario = lReader["nombreUsuario"].ToString() ?? string.Empty,
                            NombreTipoSorteoGeneral = lReader["nombreTipoSorteoGeneral"].ToString() ?? string.Empty,
                            FechaTipoSorteo = Convert.ToDateTime(lReader["fechaTipoSorteo"]),

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
                        lCmd.Parameters.Add(new SqlParameter("@id", pSorteo.Id));
                    }
                    else 
                    {
                        lCmd.Parameters.Add(new SqlParameter("@id", pSorteo.Id));
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






        public int RecIdSorteoFromParametro()
        {
            int lObjRespuesta = 0;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecIdSorteoFromPaametroPA");
                    
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto TipoSorteo
                    while (lReader.Read())
                    {
                        gObjSqlCommandAbrirCerrar.CerrarConexion(lCmd);
                    }
                        
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
