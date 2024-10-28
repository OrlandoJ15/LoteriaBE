
using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Interfaz
{
    public interface ITipoSorteoAD
    {
        List<TipoSorteo> RecTipoSorteo();
        TipoSorteo? RecTipoSorteoXId(int pIdTipoSorteo);
        bool InsTipoSorteo(TipoSorteo pTipoSorteo);
        bool ModTipoSorteo(TipoSorteo pTipoSorteo);
        bool DelTipoSorteo(TipoSorteo pTipoSorteo);

    }
}
