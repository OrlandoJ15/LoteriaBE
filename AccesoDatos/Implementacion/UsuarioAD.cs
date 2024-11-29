
using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Entidades.Models;
using MetodosComunes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;



namespace AccesoDatos.Implementacion
{

    public class UsuarioAD : IUsuarioAD
    {

        public Excepciones gObjExcepciones = new Excepciones();

        public SqlCommandAbirCerrar gObjSqlCommandAbrirCerrar = new SqlCommandAbirCerrar();

        private readonly IConfiguration _configuration;

        public UsuarioAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Usuario> RecUsuario()
        {
            List<Usuario> lObjRespuesta = new List<Usuario>();
            try
            {
                using (LoteriaContext lObjCnn = new LoteriaContext(_configuration))
                {

                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lObjCnn, "RecUsuarioPA");
                    var lReader = lCmd.ExecuteReader();
                    while (lReader.Read())
                    {
                        Usuario lobjDatosUsuario = new Usuario();
                        lobjDatosUsuario.Id = Convert.ToInt32(lReader["id"]);
                        lobjDatosUsuario.Nombre = lReader["nombre"]?.ToString() ?? string.Empty;
                        lobjDatosUsuario.NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty;
                        lobjDatosUsuario.Rol = Convert.ToInt32(lReader["rol"]);
                        lobjDatosUsuario.Correo = lReader["correo"]?.ToString() ?? string.Empty;
                        lObjRespuesta.Add(lobjDatosUsuario);
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

        public Usuario? RecUsuarioXId(int pIdUsuario)
        {
            Usuario? lObjRespuesta = null;  // Inicializamos como null
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecUsuarioXIdPA");
                    lCmd.Parameters.Add(new SqlParameter("@idUsuario", pIdUsuario));
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto Usuario
                    if (lReader.Read())
                    {
                        lObjRespuesta = new Usuario
                        {
                            Id = Convert.ToInt32(lReader["idUsuario"]),
                            Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                            NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty,
                            Rol = Convert.ToInt32(lReader["rol"]),
                            Correo = lReader["correo"]?.ToString() ?? string.Empty
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

        public bool InsUsuario(Usuario pUsuario)
        {
            return EjecutarProcedimiento("insUsuarioPA", pUsuario);
        }

        public bool ModUsuario(Usuario pUsuario)
        {
            return EjecutarProcedimiento("modUsuarioPA", pUsuario);
        }

        public bool DelUsuario(Usuario pUsuario)
        {
            return EjecutarProcedimiento("delUsuarioPA", pUsuario);
        }

        // Método auxiliar para insertar, modificar o eliminar usuario
        private bool EjecutarProcedimiento(string procedimientoAlmacenado, Usuario pUsuario)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, procedimientoAlmacenado);

                    if (procedimientoAlmacenado == "delUsuarioPA")
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idUsuario", pUsuario.Id));
                    }
                    else
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idUsuario", pUsuario.Id));
                        lCmd.Parameters.Add(new SqlParameter("@nombre", pUsuario.Nombre));
                        lCmd.Parameters.Add(new SqlParameter("@nombreUsuario", pUsuario.NombreUsuario));
                        lCmd.Parameters.Add(new SqlParameter("@rol", pUsuario.Rol));
                        lCmd.Parameters.Add(new SqlParameter("@correo", pUsuario.Correo));

                        // Solo para inserciones de usuario, se añade la clave
                        if (procedimientoAlmacenado == "insUsuarioPA")
                        {
                            lCmd.Parameters.Add(new SqlParameter("@clave", pUsuario.Clave));
                        }
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


        public Usuario? ValidarLoginUsuario(int pId, string pClave)
        {
            Usuario? lObjRespuesta = null;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "ValidarLoginUsuarioPA");
                    lCmd.Parameters.Add(new SqlParameter("@idUsuario", pId));
                    lCmd.Parameters.Add(new SqlParameter("@clave", pClave));

                    using (var lReader = lCmd.ExecuteReader())
                    {
                        // Si hay filas, significa que la consulta devolvió un resultado
                        if (lReader.Read())
                        {
                            lObjRespuesta = new Usuario
                            {
                                Id = Convert.ToInt32(lReader["id"]),
                                Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                                NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty,
                                Rol = Convert.ToInt32(lReader["rol"]),
                                Correo = lReader["correo"]?.ToString() ?? string.Empty
                            };
                        }
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


        public bool ModClaveUsuario(int pId, string pClave)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "ModClaveUsuarioPA");
                    lCmd.Parameters.Add(new SqlParameter("@idUsuario", pId));
                    lCmd.Parameters.Add(new SqlParameter("@clave", pClave));

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
                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
            return lObjRespuesta;
        }
    }
}
