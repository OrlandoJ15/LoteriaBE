using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.Extensions.Configuration;

namespace LogicaNegocio.Implementacion
{
    public class TipoSorteoGeneralLN : ITipoSorteoGeneralLN
    {
        private readonly ITipoSorteoGeneralGeneralAD gObjTipoSorteoGeneralAD;
        
        public Excepciones gObjExcepciones = new Excepciones();

        public TipoSorteoGeneralLN(IConfiguration _configuration)
        {
            gObjTipoSorteoGeneralAD = new TipoSorteoGeneralGeneralAD(_configuration);
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

        public List<TipoSorteoGeneral> RecTipoSorteoGeneral()
        {
            return EjecutarProcConEntidad(() => gObjTipoSorteoGeneralAD.RecTipoSorteoGeneral());
        }

        public TipoSorteoGeneral? RecTipoSorteoGeneralXId(int pIdTipoSorteoGeneral)
        {
            return EjecutarProcConEntidad(() => gObjTipoSorteoGeneralAD.RecTipoSorteoGeneralXId(pIdTipoSorteoGeneral));
        }

        public bool InsTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral)
        {
            return EjecutarProcSinEntidad(() => gObjTipoSorteoGeneralAD.InsTipoSorteoGeneral(pTipoSorteoGeneral));
        }

        public bool ModTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral)
        {
            return EjecutarProcSinEntidad(() => gObjTipoSorteoGeneralAD.ModTipoSorteoGeneral(pTipoSorteoGeneral));
        }

        public bool DelTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral)
        {
            return EjecutarProcSinEntidad(() => gObjTipoSorteoGeneralAD.DelTipoSorteoGeneral(pTipoSorteoGeneral));
        }
    }
}
