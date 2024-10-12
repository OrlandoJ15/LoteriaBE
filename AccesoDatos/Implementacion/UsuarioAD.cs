
using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Entidades.Models;
using NLog;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Implementacion
{
    public class UsuarioAD : IUsuarioAD
    {
        //private readonly string _configuration;



        
        private readonly Logger gObjError = LogManager.GetCurrentClassLogger();



        private readonly IConfiguration _configuration;

        public UsuarioAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*public UsuarioAD(string lCnnBD)
        {
            _configuration = lCnnBD;
        }*/
        
        public List<Usuario> RecUsuario()
        {
            List<Usuario> lObjRespuesta = new List<Usuario>();
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = lobjCnn.Database.GetDbConnection().CreateCommand();
                    lCmd.CommandText = "RecUsuarioPA";
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        lCmd.Connection.Open();
                    }
                    var lReader = lCmd.ExecuteReader();
                    while (lReader.Read())
                    {
                        Usuario lobjDatosUsuario = new Usuario();
                        lobjDatosUsuario.IdUsuario = Convert.ToInt32(lReader["idUsuario"]);
                        lobjDatosUsuario.Nombre = lReader["nombre"]?.ToString() ?? string.Empty;
                        lobjDatosUsuario.NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty;
                        lobjDatosUsuario.Rol = Convert.ToInt32(lReader["rol"]);
                        lobjDatosUsuario.Correo = lReader["correo"]?.ToString() ?? string.Empty;
                        lObjRespuesta.Add(lobjDatosUsuario);
                    }
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                    {
                        lCmd.Connection.Close();
                    }
                }
            }
            catch (Exception lEx)
            {

                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

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
                    var lCmd = lobjCnn.Database.GetDbConnection().CreateCommand();
                    lCmd.CommandText = "RecUsuarioXIdPA";
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    lCmd.Parameters.Add(new SqlParameter("@idUsuario", pIdUsuario));
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        lCmd.Connection.Open();
                    }
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto Usuario
                    if (lReader.Read())
                    {
                        lObjRespuesta = new Usuario
                        {
                            IdUsuario = Convert.ToInt32(lReader["idUsuario"]),
                            Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                            NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty,
                            Rol = Convert.ToInt32(lReader["rol"]),
                            Correo = lReader["correo"]?.ToString() ?? string.Empty  
                        };
                    }

                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                    {
                        lCmd.Connection.Close();
                    }
                }
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Lanza la excepción para que la maneje la capa superior
                throw;
            }

            // Si no se encontró ningún registro, lObjRespuesta seguirá siendo null
            return lObjRespuesta;
        }

        public bool InsUsuario(Usuario pUsuarioPA)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = lobjCnn.Database.GetDbConnection().CreateCommand();
                    lCmd.CommandText = "InsUsuarioPA";
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    lCmd.Parameters.Add(new SqlParameter("@idUsuario", pUsuarioPA.IdUsuario));
                    lCmd.Parameters.Add(new SqlParameter("@nombre", pUsuarioPA.Nombre));
                    lCmd.Parameters.Add(new SqlParameter("@nombreUsuario", pUsuarioPA.NombreUsuario));
                    lCmd.Parameters.Add(new SqlParameter("@rol", pUsuarioPA.Rol));
                    lCmd.Parameters.Add(new SqlParameter("@correo", pUsuarioPA.Correo));
                    lCmd.Parameters.Add(new SqlParameter("@clave", pUsuarioPA.Clave));
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        lCmd.Connection.Open();
                    }

                    if (lCmd.ExecuteNonQuery() > 0)
                    {
                        lObjRespuesta = true;
                    }
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                    {
                        lCmd.Connection.Close();
                    }
                }
            }
            catch (Exception lEx)
            {

                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
            return lObjRespuesta;
        }

        public bool ModUsuario(Usuario pUsuarioPA)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = lobjCnn.Database.GetDbConnection().CreateCommand();
                    lCmd.CommandText = "ModUsuarioPA";
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    lCmd.Parameters.Add(new SqlParameter("@idUsuario", pUsuarioPA.IdUsuario));
                    lCmd.Parameters.Add(new SqlParameter("@nombre", pUsuarioPA.Nombre));
                    lCmd.Parameters.Add(new SqlParameter("@nombreUsuario", pUsuarioPA.NombreUsuario));
                    lCmd.Parameters.Add(new SqlParameter("@rol", pUsuarioPA.Rol));
                    lCmd.Parameters.Add(new SqlParameter("@correo", pUsuarioPA.Correo));
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        lCmd.Connection.Open();
                    }

                    if (lCmd.ExecuteNonQuery() > 0)
                    {
                        lObjRespuesta = true;
                    }
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                    {
                        lCmd.Connection.Close();
                    }
                }
            }
            catch (Exception lEx)
            {

                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
            return lObjRespuesta;
        }

        public bool DelUsuario(Usuario pUsuarioPA)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = lobjCnn.Database.GetDbConnection().CreateCommand();
                    lCmd.CommandText = "DelUsuarioPA";
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    lCmd.Parameters.Add(new SqlParameter("@idUsuario", pUsuarioPA.IdUsuario));
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        lCmd.Connection.Open();
                    }

                    if (lCmd.ExecuteNonQuery() > 0)
                    {
                        lObjRespuesta = true;
                    }
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                    {
                        lCmd.Connection.Close();
                    }
                }
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Lanza la excepción para que la maneje la capa superior
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
                    var lCmd = lobjCnn.Database.GetDbConnection().CreateCommand();
                    lCmd.CommandText = "ValidarLoginUsuarioPA";
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    lCmd.Parameters.Add(new SqlParameter("@idUsuario", pId));
                    lCmd.Parameters.Add(new SqlParameter("@clave", pClave));
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        lCmd.Connection.Open();
                    }

                    using (var lReader = lCmd.ExecuteReader())
                    {
                        // Si hay filas, significa que la consulta devolvió un resultado
                        if (lReader.Read())
                        {
                            lObjRespuesta = new Usuario
                            {
                                IdUsuario = Convert.ToInt32(lReader["idUsuario"]),
                                Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                                NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty,
                                Rol = Convert.ToInt32(lReader["rol"]),
                                Correo = lReader["correo"]?.ToString() ?? string.Empty
                            };
                        }
                    }
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                    {
                        lCmd.Connection.Close();
                    }
                }
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

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
                    var lCmd = lobjCnn.Database.GetDbConnection().CreateCommand();
                    lCmd.CommandText = "ModClaveUsuarioPA";
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    lCmd.Parameters.Add(new SqlParameter("@idUsuario", pId));
                    lCmd.Parameters.Add(new SqlParameter("@clave", pClave));
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        lCmd.Connection.Open();
                    }

                    if (lCmd.ExecuteNonQuery() > 0)
                    {
                        lObjRespuesta = true;
                    }
                    if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                    {
                        lCmd.Connection.Close();
                    }
                }
            }
            catch (Exception lEx)
            {

                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
            return lObjRespuesta;
        }


    }
}



/*
using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Entidades.Models;
using NLog;

namespace AccesoDatos.Implementacion
{
    public class UsuarioAD : IUsuarioAD
    {
        private readonly LoteriaContext _dbContext;  // Inyección del contexto de base de datos
        private readonly Logger gObjError = LogManager.GetCurrentClassLogger();

        // Constructor que recibe LoteriaContext inyectado
        public UsuarioAD(LoteriaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Usuario> RecUsuario()
        {
            List<Usuario> lObjRespuesta = new List<Usuario>();
            try
            {
                var lCmd = _dbContext.Database.GetDbConnection().CreateCommand();
                lCmd.CommandText = "RecUsuarioPA";
                lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    lCmd.Connection.Open();
                }
                var lReader = lCmd.ExecuteReader();
                while (lReader.Read())
                {
                    Usuario lobjDatosUsuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(lReader["idUsuario"]),
                        Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                        NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty,
                        Rol = Convert.ToInt32(lReader["rol"]),
                        Correo = lReader["correo"]?.ToString() ?? string.Empty
                    };
                    lObjRespuesta.Add(lobjDatosUsuario);
                }
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                {
                    lCmd.Connection.Close();
                }
            }
            catch (Exception lEx)
            {
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible";
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);
                throw;
            }
            return lObjRespuesta;
        }

        public Usuario? RecUsuarioXId(int pIdUsuario)
        {
            Usuario? lObjRespuesta = null;
            try
            {
                var lCmd = _dbContext.Database.GetDbConnection().CreateCommand();
                lCmd.CommandText = "RecUsuarioXIdPA";
                lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                lCmd.Parameters.Add(new SqlParameter("@idUsuario", pIdUsuario));
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    lCmd.Connection.Open();
                }
                var lReader = lCmd.ExecuteReader();
                if (lReader.Read())
                {
                    lObjRespuesta = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(lReader["idUsuario"]),
                        Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                        NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty,
                        Rol = Convert.ToInt32(lReader["rol"]),
                        Correo = lReader["correo"]?.ToString() ?? string.Empty
                    };
                }
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                {
                    lCmd.Connection.Close();
                }
            }
            catch (Exception lEx)
            {
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible";
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);
                throw;
            }
            return lObjRespuesta;
        }

        public bool InsUsuario(Usuario pUsuarioPA)
        {
            bool lObjRespuesta = false;
            try
            {
                var lCmd = _dbContext.Database.GetDbConnection().CreateCommand();
                lCmd.CommandText = "InsUsuarioPA";
                lCmd.CommandType = System.Data.CommandType.StoredProcedure;
                lCmd.Parameters.Add(new SqlParameter("@idUsuario", pUsuarioPA.IdUsuario));
                lCmd.Parameters.Add(new SqlParameter("@nombre", pUsuarioPA.Nombre));
                lCmd.Parameters.Add(new SqlParameter("@nombreUsuario", pUsuarioPA.NombreUsuario));
                lCmd.Parameters.Add(new SqlParameter("@rol", pUsuarioPA.Rol));
                lCmd.Parameters.Add(new SqlParameter("@correo", pUsuarioPA.Correo));
                lCmd.Parameters.Add(new SqlParameter("@clave", pUsuarioPA.Clave));
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    lCmd.Connection.Open();
                }

                if (lCmd.ExecuteNonQuery() > 0)
                {
                    lObjRespuesta = true;
                }
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                {
                    lCmd.Connection.Close();
                }
            }
            catch (Exception lEx)
            {
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible";
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);
                throw;
            }
            return lObjRespuesta;
        }

        public bool ModUsuario(Usuario pUsuarioPA)
        {
            bool lObjRespuesta = false;
            try
            {
                
                var lCmd = _dbContext.Database.GetDbConnection().CreateCommand();
                lCmd.CommandText = "ModUsuarioPA";  // Procedimiento almacenado para modificar usuario
                lCmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Agregando parámetros con manejo de posibles valores nulos o vacíos
                lCmd.Parameters.Add(new SqlParameter("@idUsuario", pUsuarioPA.IdUsuario));
                lCmd.Parameters.Add(new SqlParameter("@nombre", pUsuarioPA.Nombre ?? (object)DBNull.Value));
                lCmd.Parameters.Add(new SqlParameter("@nombreUsuario", pUsuarioPA.NombreUsuario ?? (object)DBNull.Value));
                lCmd.Parameters.Add(new SqlParameter("@rol", pUsuarioPA.Rol));
                lCmd.Parameters.Add(new SqlParameter("@correo", pUsuarioPA.Correo ?? (object)DBNull.Value));

                // Si la contraseña se debe actualizar, la agregas así (opcional):
                // lCmd.Parameters.Add(new SqlParameter("@clave", pUsuarioPA.Clave ?? (object)DBNull.Value));

                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    lCmd.Connection.Open();
                }

                // Ejecutar el comando y verificar si se actualizó al menos un registro
                if (lCmd.ExecuteNonQuery() > 0)
                {
                    lObjRespuesta = true;
                }

                // Cerrar la conexión si está abierta
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                {
                    lCmd.Connection.Close();
                }
                
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible";

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
            return lObjRespuesta;
        }


        public bool DelUsuario(Usuario pUsuarioPA)
        {
            bool lObjRespuesta = false;

            // Validar que el id del usuario no sea nulo o negativo
            if (pUsuarioPA == null || pUsuarioPA.IdUsuario <= 0)
            {
                throw new ArgumentException("El ID del usuario debe ser un valor válido.");
            }

            try
            {
                
                var lCmd = _dbContext.Database.GetDbConnection().CreateCommand();
                lCmd.CommandText = "DelUsuarioPA";  // Procedimiento almacenado para eliminar usuario
                lCmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Agregar el parámetro ID del usuario
                lCmd.Parameters.Add(new SqlParameter("@idUsuario", pUsuarioPA.IdUsuario));

                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    lCmd.Connection.Open();
                }

                // Ejecutar el comando y verificar si se eliminó al menos un registro
                if (lCmd.ExecuteNonQuery() > 0)
                {
                    lObjRespuesta = true;
                }

                // Cerrar la conexión si está abierta
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                {
                    lCmd.Connection.Close();
                }
                
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible";

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Lanza la excepción para que la maneje la capa superior
                throw;
            }

            return lObjRespuesta;
        }

        public bool ValidarLoginUsuario(int pId, string pClave)
        {
            bool lObjRespuesta = false;

            // Validar que el ID y la clave no sean nulos o inválidos
            if (pId <= 0)
            {
                throw new ArgumentException("El ID del usuario debe ser un valor válido.");
            }

            if (string.IsNullOrWhiteSpace(pClave))
            {
                throw new ArgumentException("La clave no puede estar vacía.");
            }

            try
            {
                
                var lCmd = _dbContext.Database.GetDbConnection().CreateCommand();
                lCmd.CommandText = "ValidarLoginUsuarioPA";  // Procedimiento almacenado para validar usuario
                lCmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Agregar parámetros para el comando
                lCmd.Parameters.Add(new SqlParameter("@idUsuario", pId));
                lCmd.Parameters.Add(new SqlParameter("@clave", pClave));

                // Abrir la conexión si está cerrada
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    lCmd.Connection.Open();
                }

                // Ejecutar el comando y verificar el resultado
                if (lCmd.ExecuteNonQuery() > 0)
                {
                    lObjRespuesta = true;
                }

                // Cerrar la conexión si está abierta
                if (lCmd.Connection != null && lCmd.Connection.State == System.Data.ConnectionState.Open)
                {
                    lCmd.Connection.Close();
                }
                
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible";

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Lanza la excepción para que la maneje la capa superior
                throw;
            }

            return lObjRespuesta;
        }


    }
}
*/
