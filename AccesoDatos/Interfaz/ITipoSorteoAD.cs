
using Entidades.Models;

namespace AccesoDatos.Interfaz
{
    public interface ITipoSorteoAD
    {
        List<TipoSorteo> RecTipoSorteo();
        TipoSorteo? RecTipoSorteoXId(int pIdTipoSorteo);
        bool InsTipoSorteo(TipoSorteo pTipoSorteo);
        bool ModTipoSorteo(TipoSorteo pTipoSorteo);
        bool DelTipoSorteo(TipoSorteo pTipoSorteo);
        int RecIdTipoSorteoFromTipoSorteoGeneralOExtraordinario(int pTipoSorteo);

    }
}
