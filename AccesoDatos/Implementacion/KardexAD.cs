
using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Entidades.Models;
using MetodosComunes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Implementacion
{
    public class KardexAD : IKardexAD
    {

        public Excepciones gObjExcepciones = new Excepciones();

        public SqlCommandAbirCerrar gObjSqlCommandAbrirCerrar = new SqlCommandAbirCerrar();

        private readonly IConfiguration _configuration;

        public KardexAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public List<Kardex> RecKardex()
        {
            List<Kardex> lObjRespuesta = new List<Kardex>();
            try
            {
                using (LoteriaContext lObjCnn = new LoteriaContext(_configuration))
                {

                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lObjCnn, "RecKardexPA");
                    var lReader = lCmd.ExecuteReader();
                    while (lReader.Read())
                    {
                        Kardex lobjDatosKardex = new Kardex();
                        lobjDatosKardex.Id = Convert.ToInt32(lReader["id"]);
                        lobjDatosKardex.Serie = lReader["serie"]?.ToString() ?? string.Empty;
                        lobjDatosKardex.Numero = Convert.ToInt32(lReader["numero"]);
                        lobjDatosKardex.Nombre = lReader["nombre"]?.ToString() ?? string.Empty;
                        lobjDatosKardex.Monto = Convert.ToInt32(lReader["monto"]);
                        lobjDatosKardex.IdUsuario = Convert.ToInt32(lReader["idUsuario"]);
                        lObjRespuesta.Add(lobjDatosKardex);

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


        public Kardex? RecKardexXId(int pIdKardex)
        {
            Kardex? lObjRespuesta = null;  // Inicializamos como null
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecKardexXIdPA");
                    lCmd.Parameters.Add(new SqlParameter("@idKardex", pIdKardex));
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto Kardex
                    if (lReader.Read())
                    {
                        lObjRespuesta = new Kardex
                        {
                            Id = Convert.ToInt32(lReader["id"]),
                            Serie = lReader["serie"]?.ToString() ?? string.Empty,
                            Numero = Convert.ToInt32(lReader["numero"]),
                            Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                            Monto = Convert.ToInt32(lReader["monto"]),
                            IdUsuario = Convert.ToInt32(lReader["idUsuario"])

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

        public bool InsKardex(Kardex pKardex)
        {
            return EjecutarProcedimiento("insKardexPA", pKardex);
        }

        public bool ModKardex(Kardex pKardex)
        {
            return EjecutarProcedimiento("modKardexPA", pKardex);
        }

        public bool DelKardex(Kardex pKardex)
        {
            return EjecutarProcedimiento("delKardexPA", pKardex);
        }

        // Método auxiliar para insertar, modificar o eliminar usuario
        private bool EjecutarProcedimiento(string procedimientoAlmacenado, Kardex pKardex)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, procedimientoAlmacenado);

                    if (procedimientoAlmacenado == "delKardexPA")
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idKardex", pKardex.Id));
                    }
                    else if (procedimientoAlmacenado == "insKardexPA")
                    {
                        lCmd.Parameters.Add(new SqlParameter("@serie", pKardex.Serie));
                        lCmd.Parameters.Add(new SqlParameter("@numero", pKardex.Numero));
                        lCmd.Parameters.Add(new SqlParameter("@nombre", pKardex.Nombre));
                        lCmd.Parameters.Add(new SqlParameter("@monto", pKardex.Monto));
                        lCmd.Parameters.Add(new SqlParameter("@idUsuario", pKardex.IdUsuario));
                    }
                    else
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idKardex", pKardex.Id));
                        lCmd.Parameters.Add(new SqlParameter("@serie", pKardex.Serie));
                        lCmd.Parameters.Add(new SqlParameter("@numero", pKardex.Numero));
                        lCmd.Parameters.Add(new SqlParameter("@nombre", pKardex.Nombre));
                        lCmd.Parameters.Add(new SqlParameter("@monto", pKardex.Monto));
                        lCmd.Parameters.Add(new SqlParameter("@idUsuario", pKardex.IdUsuario));
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
