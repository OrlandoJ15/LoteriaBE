using Entidades.Models;

namespace LogicaNegocio.Interfaz
{
    public interface IDetalleSorteoLN
    {
        List<DetalleSorteo> RecDetalleSorteo();
        public DetalleSorteo? RecDetalleSorteoXId(int pIdDetalleSorteo);
        bool InsDetalleSorteo(DetalleSorteo pDetalleSorteo);
        bool ModDetalleSorteo(DetalleSorteo pDetalleSorteo);
        bool DelDetalleSorteo(DetalleSorteo pDetalleSorteo);

    }
}
