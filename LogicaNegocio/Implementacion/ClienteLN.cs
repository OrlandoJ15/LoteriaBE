using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using Microsoft.Extensions.Configuration;
using MetodosComunes;

namespace LogicaNegocio.Implementacion
{
    public class ClienteLN : IClienteLN
    {

        private readonly IClienteAD gObjClienteAD;

        public Excepciones gObjExcepciones = new Excepciones();

        public ClienteLN(IConfiguration _configuration)
        {
            gObjClienteAD = new ClienteAD(_configuration);
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

        public List<Cliente> RecCliente()
        {
            return EjecutarProcConEntidad(() => gObjClienteAD.RecCliente());
        }

        public Cliente? RecClienteXId(int pIdCliente)
        {
            return EjecutarProcConEntidad(() => gObjClienteAD.RecClienteXId(pIdCliente));
        }
        public bool InsCliente(Cliente pCliente)
        {
            return EjecutarProcSinEntidad (() => gObjClienteAD.InsCliente(pCliente));
        }

        public bool ModCliente(Cliente pCliente)
        {
            return EjecutarProcSinEntidad (() => gObjClienteAD.ModCliente(pCliente));
        }

        public bool DelCliente(Cliente pCliente)
        {
            return EjecutarProcSinEntidad (() => gObjClienteAD.DelCliente(pCliente));
        }

        public Cliente? ValidarLoginCliente(int pId, string pClave)
        {
            return EjecutarProcConEntidad (() => gObjClienteAD.ValidarLoginCliente (pId, pClave));
        }

        public bool ModClaveCliente(int pId, string pClave)
        {
            return EjecutarProcSinEntidad (() => gObjClienteAD.ModClaveCliente (pId, pClave));
        }

    }
}

