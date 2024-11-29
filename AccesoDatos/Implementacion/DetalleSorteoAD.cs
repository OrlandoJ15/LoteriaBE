using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Entidades.Models;
using MetodosComunes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Implementacion
{
    public class DetalleSorteoAD : IDetalleSorteoAD
    {

        public Excepciones gObjExcepciones = new Excepciones();

        public SqlCommandAbirCerrar gObjSqlCommandAbrirCerrar = new SqlCommandAbirCerrar();

        private readonly IConfiguration _configuration;

        public DetalleSorteoAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public List<DetalleSorteo> RecDetalleSorteo()
        {
            List<DetalleSorteo> lObjRespuesta = new List<DetalleSorteo>();
            try
            {
                using (LoteriaContext lObjCnn = new LoteriaContext(_configuration))
                {

                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lObjCnn, "RecDetalleSorteoPA");
                    var lReader = lCmd.ExecuteReader();
                    while (lReader.Read())
                    {
                        DetalleSorteo lobjDatosDetalleSorteo = new DetalleSorteo();
                        lobjDatosDetalleSorteo.Id = Convert.ToInt32(lReader["id"]);
                        lobjDatosDetalleSorteo.IdSorteo = Convert.ToInt32(lReader["idSorteo"]);
                        lobjDatosDetalleSorteo.Numero = Convert.ToInt32(lReader["numero"]);
                        lobjDatosDetalleSorteo.Monto = Convert.ToInt32(lReader["monto"]);
                        lObjRespuesta.Add(lobjDatosDetalleSorteo);

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


        public DetalleSorteo? RecDetalleSorteoXId(int pIdDetalleSorteo)
        {
            DetalleSorteo? lObjRespuesta = null;  // Inicializamos como null
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecDetalleSorteoXIdPA");
                    lCmd.Parameters.Add(new SqlParameter("@idDetalleSorteo", pIdDetalleSorteo));
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto DetalleSorteo
                    if (lReader.Read())
                    {
                        lObjRespuesta = new DetalleSorteo
                        {
                            Id = Convert.ToInt32(lReader["id"]),
                            IdSorteo = Convert.ToInt32(lReader["idSorteo"]),
                            Numero = Convert.ToInt32(lReader["numero"]),
                            Monto = Convert.ToInt32(lReader["monto"]),
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

        public bool InsDetalleSorteo(DetalleSorteo pDetalleSorteo)
        {
            return EjecutarProcedimiento("insDetalleSorteoPA", pDetalleSorteo);
        }

        public bool ModDetalleSorteo(DetalleSorteo pDetalleSorteo)
        {
            return EjecutarProcedimiento("modDetalleSorteoPA", pDetalleSorteo);
        }

        public bool DelDetalleSorteo(DetalleSorteo pDetalleSorteo)
        {
            return EjecutarProcedimiento("delDetalleSorteoPA", pDetalleSorteo);
        }

        // Método auxiliar para insertar, modificar o eliminar usuario
        private bool EjecutarProcedimiento(string procedimientoAlmacenado, DetalleSorteo pDetalleSorteo)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, procedimientoAlmacenado);

                    if (procedimientoAlmacenado == "delDetalleSorteoPA")
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idDetalleSorteo", pDetalleSorteo.Id));
                    }
                    else
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idDetalleSorteo", pDetalleSorteo.Id));
                        lCmd.Parameters.Add(new SqlParameter("@idSorteo", pDetalleSorteo.IdSorteo));
                        lCmd.Parameters.Add(new SqlParameter("@numero", pDetalleSorteo.Numero));
                        lCmd.Parameters.Add(new SqlParameter("@monto", pDetalleSorteo.Monto));

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
