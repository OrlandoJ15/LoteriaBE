using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Interfaz
{
    public interface ITipoSorteoLN
    {
        List<TipoSorteo> RecTipoSorteo();
        public TipoSorteo? RecTipoSorteoXId(int pIdTipoSorteo);
        bool InsTipoSorteo(TipoSorteo pTipoSorteo);
        bool ModTipoSorteo(TipoSorteo pTipoSorteo);
        bool DelTipoSorteo(TipoSorteo pTipoSorteo);

    }
}
