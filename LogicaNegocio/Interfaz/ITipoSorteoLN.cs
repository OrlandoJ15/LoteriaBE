using Entidades.Models;

namespace LogicaNegocio.Interfaz
{
    public interface ITipoSorteoLN
    {
        List<TipoSorteo> RecTipoSorteo();
        public TipoSorteo? RecTipoSorteoXId(int pIdTipoSorteo);
        bool InsTipoSorteo(TipoSorteo pTipoSorteo);
        bool ModTipoSorteo(TipoSorteo pTipoSorteo);
        bool DelTipoSorteo(TipoSorteo pTipoSorteo);
        int RecIdTipoSorteoFromTipoSorteoGeneralOExtraordinario(int pIdTipoSorteoGeneral);

    }
}
