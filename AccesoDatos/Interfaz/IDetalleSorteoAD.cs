using Entidades.Models;

namespace AccesoDatos.Interfaz
{
    public interface IDetalleSorteoAD
    {
        List<DetalleSorteo> RecDetalleSorteo();
        DetalleSorteo? RecDetalleSorteoXId(int pIdDetalleSorteo);
        bool InsDetalleSorteo(DetalleSorteo pDetalleSorteo);
        bool ModDetalleSorteo(DetalleSorteo pDetalleSorteo);
        bool DelDetalleSorteo(DetalleSorteo pDetalleSorteo);
    }
}

