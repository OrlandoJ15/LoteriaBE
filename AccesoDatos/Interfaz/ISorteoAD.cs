using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Interfaz
{
    public interface ISorteoAD
    {
        List<Sorteo> RecSorteo();
        Sorteo? RecSorteoXId(int pIdSorteo);
        bool InsSorteo(Sorteo pSorteo);
        bool ModSorteo(Sorteo pSorteo);
        bool DelSorteo(Sorteo pSorteo);
    }
}

