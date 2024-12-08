using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.Extensions.Configuration;

namespace LogicaNegocio.Implementacion
{
    public class TipoSorteoGeneralExtraordinarioLN : ITipoSorteoGeneralExtraordinarioLN
    {
        private readonly ITipoSorteoGeneralExtraordinarioAD gObjTipoSorteoGeneralExtraordinarioAD;
        
        public Excepciones gObjExcepciones = new Excepciones();

        public TipoSorteoGeneralExtraordinarioLN(IConfiguration _configuration)
        {
            gObjTipoSorteoGeneralExtraordinarioAD = new TipoSorteoGeneralExtraordinarioAD(_configuration);
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

        public List<TipoSorteoGeneralExtraordinario> RecTipoSorteoGeneralExtraordinario()
        {
            return EjecutarProcConEntidad(() => gObjTipoSorteoGeneralExtraordinarioAD.RecTipoSorteoGeneralExtraordinario());
        }

        public TipoSorteoGeneralExtraordinario? RecTipoSorteoGeneralExtraordinarioXId(int pIdTipoSorteoGeneralExtraordinario)
        {
            return EjecutarProcConEntidad(() => gObjTipoSorteoGeneralExtraordinarioAD.RecTipoSorteoGeneralExtraordinarioXId(pIdTipoSorteoGeneralExtraordinario));
        }

        public bool InsTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            return EjecutarProcSinEntidad(() => gObjTipoSorteoGeneralExtraordinarioAD.InsTipoSorteoGeneralExtraordinario(pTipoSorteoGeneralExtraordinario));
        }

        public bool ModTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            return EjecutarProcSinEntidad(() => gObjTipoSorteoGeneralExtraordinarioAD.ModTipoSorteoGeneralExtraordinario(pTipoSorteoGeneralExtraordinario));
        }

        public bool DelTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            return EjecutarProcSinEntidad(() => gObjTipoSorteoGeneralExtraordinarioAD.DelTipoSorteoGeneralExtraordinario(pTipoSorteoGeneralExtraordinario));
        }
    }
}
