using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Interfaz
{
    public interface IKardexLN
    {
        List<Kardex> RecKardex();
        public Kardex? RecKardexXId(int pIdKardex);
        bool InsKardex(Kardex pKardex);
        bool ModKardex(Kardex pKardex);
        bool DelKardex(Kardex pKardex);

    }
}
