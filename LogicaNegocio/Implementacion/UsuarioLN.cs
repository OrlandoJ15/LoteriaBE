using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using Microsoft.Extensions.Configuration;
using MetodosComunes;

namespace LogicaNegocio.Implementacion
{
    public class UsuarioLN : IUsuarioLN
    {

        private readonly IUsuarioAD gObjUsuarioAD;

        public Excepciones gObjExcepciones = new Excepciones();

        public UsuarioLN(IConfiguration _configuration)
        {
            gObjUsuarioAD = new UsuarioAD(_configuration);
        }


        private T EjecutarProcConEntidad<T> (Func<T> funcion)
        {
            try
            {
                return funcion();
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
        }
        private bool EjecutarProcSinEntidad(Action accion)
        {
            try
            {
                accion();
                return true;
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
        }

        public List<Usuario> RecUsuario()
        {
            return EjecutarProcConEntidad(() => gObjUsuarioAD.RecUsuario());
        }

        public Usuario? RecUsuarioXId(int pIdUsuario)
        {
            return EjecutarProcConEntidad(() => gObjUsuarioAD.RecUsuarioXId(pIdUsuario));
        }
        public bool InsUsuario(Usuario pUsuario)
        {
            return EjecutarProcSinEntidad (() => gObjUsuarioAD.InsUsuario(pUsuario));
        }

        public bool ModUsuario(Usuario pUsuario)
        {
            return EjecutarProcSinEntidad (() => gObjUsuarioAD.ModUsuario(pUsuario));
        }

        public bool DelUsuario(Usuario pUsuario)
        {
            return EjecutarProcSinEntidad (() => gObjUsuarioAD.DelUsuario(pUsuario));
        }

        public Usuario? ValidarLoginUsuario(int pId, string pClave)
        {
            return EjecutarProcConEntidad (() => gObjUsuarioAD.ValidarLoginUsuario (pId, pClave));
        }

        public bool ModClaveUsuario(int pId, string pClave)
        {
            return EjecutarProcSinEntidad (() => gObjUsuarioAD.ModClaveUsuario (pId, pClave));
        }

    }
}









/*
using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using Microsoft.Extensions.Configuration;
using MetodosComunes;

namespace LogicaNegocio.Implementacion
{
    public class UsuarioLN : IUsuarioLN
    {
        
        private readonly IUsuarioAD gObjUsuarioAD;

        public Excepciones gObjExcepciones = new Excepciones();

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
                gObjExcepciones.LogError(lEx);
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
                gObjExcepciones.LogError(lEx);
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
                gObjExcepciones.LogError(lEx);
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
                gObjExcepciones.LogError(lEx);
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
                gObjExcepciones.LogError(lEx);
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
                lObjRespuesta = gObjUsuarioAD.ModClaveUsuario(pId, pClave);
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
*/
