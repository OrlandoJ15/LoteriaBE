using Entidades.Models;

namespace LogicaNegocio.Interfaz
{
    public interface ISorteoLN
    {
        List<Sorteo> RecSorteo();
        public Sorteo? RecSorteoXId(int pIdSorteo);
        bool InsSorteo(Sorteo pSorteo);
        bool ModSorteo(Sorteo pSorteo);
        bool DelSorteo(Sorteo pSorteo);
        public int RecIdSorteoFromParametro();

    }
}
