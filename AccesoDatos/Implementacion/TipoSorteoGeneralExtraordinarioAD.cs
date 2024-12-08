using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Entidades.Models;
using MetodosComunes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Implementacion
{
    public class TipoSorteoGeneralExtraordinarioAD : ITipoSorteoGeneralExtraordinarioAD
    {

        public Excepciones gObjExcepciones = new Excepciones();

        public SqlCommandAbirCerrar gObjSqlCommandAbrirCerrar = new SqlCommandAbirCerrar();

        private readonly IConfiguration _configuration;

        public TipoSorteoGeneralExtraordinarioAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public List<TipoSorteoGeneralExtraordinario> RecTipoSorteoGeneralExtraordinario()
        {
            List<TipoSorteoGeneralExtraordinario> lObjRespuesta = new List<TipoSorteoGeneralExtraordinario>();
            try
            {
                using (LoteriaContext lObjCnn = new LoteriaContext(_configuration))
                {

                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lObjCnn, "RecTipoSorteoGeneralExtraordinarioPA");
                    var lReader = lCmd.ExecuteReader();
                    while (lReader.Read())
                    {
                        TipoSorteoGeneralExtraordinario lobjDatosTipoSorteoGeneralExtraordinario = new TipoSorteoGeneralExtraordinario();
                        lobjDatosTipoSorteoGeneralExtraordinario.Id = Convert.ToInt32(lReader["id"]);
                        lobjDatosTipoSorteoGeneralExtraordinario.Nombre = lReader["nombre"]?.ToString() ?? string.Empty;
                        lobjDatosTipoSorteoGeneralExtraordinario.Fondo = Convert.ToInt32(lReader["fondo"]);
                        lobjDatosTipoSorteoGeneralExtraordinario.PorcentajePago = Convert.ToInt32(lReader["porcentajePago"]);
                        lobjDatosTipoSorteoGeneralExtraordinario.FechaInicio = Convert.ToDateTime(lReader["fechaInicio"]);
                        lobjDatosTipoSorteoGeneralExtraordinario.FechaFin = Convert.ToDateTime(lReader["fechaFin"]);
                        lObjRespuesta.Add(lobjDatosTipoSorteoGeneralExtraordinario);

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


        public TipoSorteoGeneralExtraordinario? RecTipoSorteoGeneralExtraordinarioXId(int pIdTipoSorteoGeneralExtraordinario)
        {
            TipoSorteoGeneralExtraordinario? lObjRespuesta = null;  // Inicializamos como null
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecTipoSorteoGeneralExtraordinarioXIdPA");
                    lCmd.Parameters.Add(new SqlParameter("@id", pIdTipoSorteoGeneralExtraordinario));
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto TipoSorteoGeneral
                    if (lReader.Read())
                    {
                        lObjRespuesta = new TipoSorteoGeneralExtraordinario
                        {
                            Id = Convert.ToInt32(lReader["id"]),
                            Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                            Fondo = Convert.ToInt32(lReader["fondo"]),
                            PorcentajePago = Convert.ToInt32(lReader["porcentajePago"]),
                            FechaInicio = Convert.ToDateTime(lReader["fechaInicio"]),
                            FechaFin = Convert.ToDateTime(lReader["fechaFin"])

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

        public bool InsTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            return EjecutarProcedimiento("insTipoSorteoGeneralExtraordinarioPA", pTipoSorteoGeneralExtraordinario);
        }

        public bool ModTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            return EjecutarProcedimiento("modTipoSorteoGeneralExtraordinarioPA", pTipoSorteoGeneralExtraordinario);
        }

        public bool DelTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            return EjecutarProcedimiento("delTipoSorteoGeneralExtraordinarioPA", pTipoSorteoGeneralExtraordinario);
        }

        // Método auxiliar para insertar, modificar o eliminar usuario
        private bool EjecutarProcedimiento(string procedimientoAlmacenado, TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, procedimientoAlmacenado);

                    if (procedimientoAlmacenado == "delTipoSorteoGeneralExtraordinarioPA")
                    {
                        lCmd.Parameters.Add(new SqlParameter("@id", pTipoSorteoGeneralExtraordinario.Id));
                    }
                    else
                    {
                        lCmd.Parameters.Add(new SqlParameter("@id", pTipoSorteoGeneralExtraordinario.Id));
                        lCmd.Parameters.Add(new SqlParameter("@nombre", pTipoSorteoGeneralExtraordinario.Nombre));
                        lCmd.Parameters.Add(new SqlParameter("@fondo", pTipoSorteoGeneralExtraordinario.Fondo));
                        lCmd.Parameters.Add(new SqlParameter("@porcentajePago", pTipoSorteoGeneralExtraordinario.PorcentajePago));
                        lCmd.Parameters.Add(new SqlParameter("@fechaInicio", pTipoSorteoGeneralExtraordinario.FechaInicio));
                        lCmd.Parameters.Add(new SqlParameter("@fechaFin", pTipoSorteoGeneralExtraordinario.FechaFin));

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
