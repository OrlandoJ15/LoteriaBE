using Entidades.Models;

namespace AccesoDatos.Interfaz
{
    public interface ITipoSorteoGeneralGeneralAD
    {
        List<TipoSorteoGeneral> RecTipoSorteoGeneral();
        TipoSorteoGeneral? RecTipoSorteoGeneralXId(int pIdTipoSorteoGeneral);
        bool InsTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral);
        bool ModTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral);
        bool DelTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral);

    }
}
