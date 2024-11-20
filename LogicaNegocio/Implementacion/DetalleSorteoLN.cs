using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using Entidades.Models;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.Extensions.Configuration;

namespace LogicaNegocio.Implementacion
{
    public class DetalleSorteoLN : IDetalleSorteoLN
    {

        private readonly IDetalleSorteoAD gObjDetalleSorteoAD;

        public Excepciones gObjExcepciones = new Excepciones();

        public DetalleSorteoLN(IConfiguration _configuration)
        {
            gObjDetalleSorteoAD = new DetalleSorteoAD(_configuration);
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

        public List<DetalleSorteo> RecDetalleSorteo()
        {
            return EjecutarProcConEntidad(() => gObjDetalleSorteoAD.RecDetalleSorteo());
        }

        public DetalleSorteo? RecDetalleSorteoXId(int pIdDetalleSorteo)
        {
            return EjecutarProcConEntidad(() => gObjDetalleSorteoAD.RecDetalleSorteoXId(pIdDetalleSorteo));
        }

        public bool InsDetalleSorteo(DetalleSorteo pDetalleSorteo)
        {
            return EjecutarProcSinEntidad(() => gObjDetalleSorteoAD.InsDetalleSorteo(pDetalleSorteo));
        }

        public bool ModDetalleSorteo(DetalleSorteo pDetalleSorteo)
        {
            return EjecutarProcSinEntidad(() => gObjDetalleSorteoAD.ModDetalleSorteo(pDetalleSorteo));
        }

        public bool DelDetalleSorteo(DetalleSorteo pDetalleSorteo)
        {
            return EjecutarProcSinEntidad(() => gObjDetalleSorteoAD.DelDetalleSorteo(pDetalleSorteo));
        }

    }
}
