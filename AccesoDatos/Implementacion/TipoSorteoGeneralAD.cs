using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Entidades.Models;
using MetodosComunes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Implementacion
{
    public class TipoSorteoGeneralGeneralAD : ITipoSorteoGeneralGeneralAD
    {

        public Excepciones gObjExcepciones = new Excepciones();

        public SqlCommandAbirCerrar gObjSqlCommandAbrirCerrar = new SqlCommandAbirCerrar();

        private readonly IConfiguration _configuration;

        public TipoSorteoGeneralGeneralAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public List<TipoSorteoGeneral> RecTipoSorteoGeneral()
        {
            List<TipoSorteoGeneral> lObjRespuesta = new List<TipoSorteoGeneral>();
            try
            {
                using (LoteriaContext lObjCnn = new LoteriaContext(_configuration))
                {

                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lObjCnn, "RecTipoSorteoGeneralPA");
                    var lReader = lCmd.ExecuteReader();
                    while (lReader.Read())
                    {
                        TipoSorteoGeneral lobjDatosTipoSorteoGeneral = new TipoSorteoGeneral();
                        lobjDatosTipoSorteoGeneral.Id = Convert.ToInt32(lReader["id"]);
                        lobjDatosTipoSorteoGeneral.Nombre = lReader["nombre"]?.ToString() ?? string.Empty;
                        lobjDatosTipoSorteoGeneral.Fondo = Convert.ToInt32(lReader["fondo"]);
                        lobjDatosTipoSorteoGeneral.PorcentajePago = Convert.ToInt32(lReader["porcentajePago"]);
                        lobjDatosTipoSorteoGeneral.FechaInicio = Convert.ToDateTime(lReader["fechaInicio"]);
                        lobjDatosTipoSorteoGeneral.FechaFin = Convert.ToDateTime(lReader["fechaFin"]);
                        lobjDatosTipoSorteoGeneral.IdTipoSorteoGeneralExtraordinario = Convert.ToInt32(lReader["idTipoSorteoGeneralExtraordinario"]);


                        lObjRespuesta.Add(lobjDatosTipoSorteoGeneral);

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


        public TipoSorteoGeneral? RecTipoSorteoGeneralXId(int pIdTipoSorteoGeneral)
        {
            TipoSorteoGeneral? lObjRespuesta = null;  // Inicializamos como null
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecTipoSorteoGeneralXIdPA");
                    lCmd.Parameters.Add(new SqlParameter("@id", pIdTipoSorteoGeneral));
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto TipoSorteoGeneral
                    if (lReader.Read())
                    {
                        lObjRespuesta = new TipoSorteoGeneral
                        {
                            Id = Convert.ToInt32(lReader["id"]),
                            Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                            Fondo = Convert.ToInt32(lReader["fondo"]),
                            PorcentajePago = Convert.ToInt32(lReader["porcentajePago"]),
                            FechaInicio = Convert.ToDateTime(lReader["fechaInicio"]),
                            FechaFin = Convert.ToDateTime(lReader["fechaFin"]),
                            IdTipoSorteoGeneralExtraordinario = Convert.ToInt32(lReader["idTipoSorteoGeneralExtraordinario"])
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

        public bool InsTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral)
        {
            return EjecutarProcedimiento("insTipoSorteoGeneralPA", pTipoSorteoGeneral);
        }

        public bool ModTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral)
        {
            return EjecutarProcedimiento("modTipoSorteoGeneralPA", pTipoSorteoGeneral);
        }

        public bool DelTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral)
        {
            return EjecutarProcedimiento("delTipoSorteoGeneralPA", pTipoSorteoGeneral);
        }

        // Método auxiliar para insertar, modificar o eliminar usuario
        private bool EjecutarProcedimiento(string procedimientoAlmacenado, TipoSorteoGeneral pTipoSorteoGeneral)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, procedimientoAlmacenado);

                    if (procedimientoAlmacenado == "delTipoSorteoGeneralPA")
                    {
                        lCmd.Parameters.Add(new SqlParameter("@id", pTipoSorteoGeneral.Id));
                    }
                    else
                    {
                        lCmd.Parameters.Add(new SqlParameter("@id", pTipoSorteoGeneral.Id));
                        lCmd.Parameters.Add(new SqlParameter("@nombre", pTipoSorteoGeneral.Nombre));
                        lCmd.Parameters.Add(new SqlParameter("@fondo", pTipoSorteoGeneral.Fondo));
                        lCmd.Parameters.Add(new SqlParameter("@porcentajePago", pTipoSorteoGeneral.PorcentajePago));
                        lCmd.Parameters.Add(new SqlParameter("@fechaInicio", pTipoSorteoGeneral.FechaInicio));
                        lCmd.Parameters.Add(new SqlParameter("@fechaFin", pTipoSorteoGeneral.FechaFin));
                        lCmd.Parameters.Add(new SqlParameter("@idTipoSorteoGeneralExtraordinario", pTipoSorteoGeneral.IdTipoSorteoGeneralExtraordinario));

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
