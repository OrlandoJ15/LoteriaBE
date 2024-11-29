using Entidades.Models;

namespace LogicaNegocio.Interfaz
{
    public interface ITipoSorteoGeneralLN
    {
        List<TipoSorteoGeneral> RecTipoSorteoGeneral();
        public TipoSorteoGeneral? RecTipoSorteoGeneralXId(int pIdTipoSorteoGeneral);
        bool InsTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral);
        bool ModTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral);
        bool DelTipoSorteoGeneral(TipoSorteoGeneral pTipoSorteoGeneral);

    }
}
