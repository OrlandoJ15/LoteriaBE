
using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using Microsoft.Extensions.Configuration;
using NLog;


namespace LogicaNegocio.Implementacion
{
    public class UsuarioLN : IUsuarioLN
    {
        
        private readonly IUsuarioAD gObjUsuarioAD;
     

        private readonly Logger gObjError = LogManager.GetCurrentClassLogger();

        public UsuarioLN(IConfiguration _configuration)
        {
            gObjUsuarioAD = new UsuarioAD(_configuration);
        }

        public List<Usuario> RecUsuario()
        {
            List<Usuario> lObjRespuesta = new List<Usuario>();

            try
            {
                lObjRespuesta = gObjUsuarioAD.RecUsuario();
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
            Usuario? lObjRespuesta = null;

            try
            {
                lObjRespuesta = gObjUsuarioAD.RecUsuarioXId(pIdUsuario);
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
        public bool InsUsuario(Usuario pUsuario)
        {
            bool lObjRespuesta = false;

            try
            {
                lObjRespuesta = gObjUsuarioAD.InsUsuario(pUsuario);
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

        public bool ModUsuario(Usuario pUsuario)
        {
            bool lObjRespuesta = false;

            try
            {
                lObjRespuesta = gObjUsuarioAD.ModUsuario(pUsuario);
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

        public bool DelUsuario(Usuario pUsuario)
        {
            bool lObjRespuesta = false;

            try
            {
                lObjRespuesta = gObjUsuarioAD.DelUsuario(pUsuario);
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
            Usuario? lObjRespuesta = null ;

            try
            {
                lObjRespuesta = gObjUsuarioAD.ValidarLoginUsuario(pId, pClave);
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
                lObjRespuesta = gObjUsuarioAD.ModClaveUsuario(pId, pClave);
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
using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using NLog;

namespace LogicaNegocio.Implementacion
{
    public class UsuarioLN : IUsuarioLN
    {
        private readonly IUsuarioAD gObjUsuarioAD;
        private readonly Logger gObjError = LogManager.GetCurrentClassLogger();

        public UsuarioLN(IUsuarioAD usuarioAD)
        {
            gObjUsuarioAD = usuarioAD ?? throw new ArgumentNullException(nameof(usuarioAD), "El acceso a datos no puede ser nulo.");
        }

        public List<Usuario> RecUsuario()
        {
            try
            {
                return gObjUsuarioAD.RecUsuario();
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                throw;
            }
        }

        public Usuario? RecUsuarioXId(int pIdUsuario)
        {
            // Validar que el ID del usuario sea un valor válido
            if (pIdUsuario <= 0)
            {
                throw new ArgumentException("El ID del usuario debe ser un valor positivo.");
            }

            try
            {
                return gObjUsuarioAD.RecUsuarioXId(pIdUsuario);
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                throw;
            }
        }

        public bool InsUsuario(Usuario pUsuario)
        {
            // Validar que el usuario no sea nulo
            if (pUsuario == null)
            {
                throw new ArgumentNullException(nameof(pUsuario), "El usuario no puede ser nulo.");
            }

            try
            {
                return gObjUsuarioAD.InsUsuario(pUsuario);
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                throw;
            }
        }

        public bool ModUsuario(Usuario pUsuario)
        {
            // Validar que el usuario no sea nulo
            if (pUsuario == null)
            {
                throw new ArgumentNullException(nameof(pUsuario), "El usuario no puede ser nulo.");
            }

            try
            {
                return gObjUsuarioAD.ModUsuario(pUsuario);
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                throw;
            }
        }

        public bool DelUsuario(Usuario pUsuario)
        {
            // Validar que el usuario no sea nulo
            if (pUsuario == null)
            {
                throw new ArgumentNullException(nameof(pUsuario), "El usuario no puede ser nulo.");
            }

            try
            {
                return gObjUsuarioAD.DelUsuario(pUsuario);
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                throw;
            }
        }

        public bool ValidarLoginUsuario(int pId, string pClave)
        {
            // Validar que el ID y la clave no sean nulos o inválidos
            if (pId <= 0)
            {
                throw new ArgumentException("El ID del usuario debe ser un valor positivo.");
            }
            if (string.IsNullOrWhiteSpace(pClave))
            {
                throw new ArgumentException("La clave no puede estar vacía.");
            }

            try
            {
                return gObjUsuarioAD.ValidarLoginUsuario(pId, pClave);
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                throw;
            }
        }

        private void LogError(Exception lEx)
        {
            // Obtener el nombre del método actual de forma segura
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            string methodName = methodInfo?.ToString() ?? "Método no disponible";

            // Registrar el error
            gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                            "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                            ". Método: " + methodName);
        }
    }
}

*/