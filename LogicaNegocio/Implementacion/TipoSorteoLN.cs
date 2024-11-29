using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.Extensions.Configuration;

namespace LogicaNegocio.Implementacion
{
    public class TipoSorteoLN : ITipoSorteoLN
    {

        private readonly ITipoSorteoAD gObjTipoSorteoAD;

        public Excepciones gObjExcepciones = new Excepciones();

        public TipoSorteoLN(IConfiguration _configuration)
        {
            gObjTipoSorteoAD = new TipoSorteoAD(_configuration);
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

        public List<TipoSorteo> RecTipoSorteo()
        {
            return EjecutarProcConEntidad(() => gObjTipoSorteoAD.RecTipoSorteo());
        }

        public TipoSorteo? RecTipoSorteoXId(int pIdTipoSorteo)
        {
            return EjecutarProcConEntidad(() => gObjTipoSorteoAD.RecTipoSorteoXId(pIdTipoSorteo));
        }

        public bool InsTipoSorteo(TipoSorteo pTipoSorteo)
        {
            return EjecutarProcSinEntidad(() => gObjTipoSorteoAD.InsTipoSorteo(pTipoSorteo));
        }

        public bool ModTipoSorteo(TipoSorteo pTipoSorteo)
        {
            return EjecutarProcSinEntidad(() => gObjTipoSorteoAD.ModTipoSorteo(pTipoSorteo));
        }

        public bool DelTipoSorteo(TipoSorteo pTipoSorteo)
        {
            return EjecutarProcSinEntidad(() => gObjTipoSorteoAD.DelTipoSorteo(pTipoSorteo));
        }









        public int RecIdTipoSorteoFromTipoSorteoGeneral(int pIdTipoSorteoGeneral)
        {
            return EjecutarProcConEntidad(() => gObjTipoSorteoAD.RecIdTipoSorteoFromTipoSorteoGeneral(pIdTipoSorteoGeneral));
        }

    }
}
