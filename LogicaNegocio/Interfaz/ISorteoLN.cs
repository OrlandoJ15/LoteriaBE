using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Interfaz
{
    public interface ISorteoLN
    {
        List<Sorteo> RecSorteo();
        public Sorteo? RecSorteoXId(int pIdSorteo);
        bool InsSorteo(Sorteo pSorteo);
        bool ModSorteo(Sorteo pSorteo);
        bool DelSorteo(Sorteo pSorteo);

    }
}
