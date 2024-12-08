using Entidades.Models;

namespace LogicaNegocio.Interfaz
{
    public interface ITipoSorteoGeneralExtraordinarioLN
    {
        List<TipoSorteoGeneralExtraordinario> RecTipoSorteoGeneralExtraordinario();
        TipoSorteoGeneralExtraordinario? RecTipoSorteoGeneralExtraordinarioXId(int pIdTipoSorteoGeneralExtraordinario);
        bool InsTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario);
        bool ModTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario);
        bool DelTipoSorteoGeneralExtraordinario(TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario);


    }
}
