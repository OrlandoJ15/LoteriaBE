using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.Extensions.Configuration;

namespace LogicaNegocio.Implementacion
{
    public class SorteoLN : ISorteoLN
    {

        private readonly ISorteoAD gObjSorteoAD;

        public Excepciones gObjExcepciones = new Excepciones();

        public SorteoLN(IConfiguration _configuration)
        {
            gObjSorteoAD = new SorteoAD(_configuration);
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

        public List<Sorteo> RecSorteo()
        {
            return EjecutarProcConEntidad(() => gObjSorteoAD.RecSorteo());
        }

        public Sorteo? RecSorteoXId(int pIdSorteo)
        {
            return EjecutarProcConEntidad(() => gObjSorteoAD.RecSorteoXId(pIdSorteo));
        }

        public bool InsSorteo(Sorteo pSorteo)
        {
            return EjecutarProcSinEntidad(() => gObjSorteoAD.InsSorteo(pSorteo));
        }

        public bool ModSorteo(Sorteo pSorteo)
        {
            return EjecutarProcSinEntidad(() => gObjSorteoAD.ModSorteo(pSorteo));
        }

        public bool DelSorteo(Sorteo pSorteo)
        {
            return EjecutarProcSinEntidad(() => gObjSorteoAD.DelSorteo(pSorteo));
        }




        public int RecIdSorteoFromParametro()
        {
            return EjecutarProcConEntidad(() => gObjSorteoAD.RecIdSorteoFromParametro());
        }

    }
}
