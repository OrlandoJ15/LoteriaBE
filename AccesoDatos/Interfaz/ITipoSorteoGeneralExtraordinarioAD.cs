using Entidades.Models;

namespace AccesoDatos.Interfaz
{
    public interface ITipoSorteoGeneralExtraordinarioAD
    {
        List<TipoSorteoGeneralExtraordinario> RecTipoSorteoGeneralExtraordinario();
        TipoSorteoGeneralExtraordinario? RecTipoSorteoGeneralExtraordinarioXId(int pIdTipoSorteoGeneralExtraordinario);
        bool InsTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario);
        bool ModTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario);
        bool DelTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario);

    }
}
