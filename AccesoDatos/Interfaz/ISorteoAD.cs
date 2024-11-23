using Entidades.Models;

namespace AccesoDatos.Interfaz
{
    public interface ISorteoAD
    {
        List<Sorteo> RecSorteo();
        Sorteo? RecSorteoXId(int pIdSorteo);
        bool InsSorteo(Sorteo pSorteo);
        bool ModSorteo(Sorteo pSorteo);
        bool DelSorteo(Sorteo pSorteo);
        int RecIdSorteoFromParametro();
    }
}

