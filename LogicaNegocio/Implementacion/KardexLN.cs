using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.Extensions.Configuration;

namespace LogicaNegocio.Implementacion
{
    public class KardexLN : IKardexLN
    {

        private readonly IKardexAD gObjKardexAD;

        public Excepciones gObjExcepciones = new Excepciones();

        public KardexLN(IConfiguration _configuration)
        {
            gObjKardexAD = new KardexAD(_configuration);
        }

        private T EjecutarProcConEntidad<T>(Func<T> funcion)
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

        public List<Kardex> RecKardex()
        {
            return EjecutarProcConEntidad(() => gObjKardexAD.RecKardex());
        }

        public Kardex? RecKardexXId(int pIdKardex)
        {
            return EjecutarProcConEntidad(() => gObjKardexAD.RecKardexXId(pIdKardex));
        }

        public bool InsKardex(Kardex pKardex)
        {
            return EjecutarProcSinEntidad(() => gObjKardexAD.InsKardex(pKardex));
        }

        public bool ModKardex(Kardex pKardex)
        {
            return EjecutarProcSinEntidad(() => gObjKardexAD.ModKardex(pKardex));
        }

        public bool DelKardex(Kardex pKardex)
        {
            return EjecutarProcSinEntidad(() => gObjKardexAD.DelKardex(pKardex));
        }

    }
}
