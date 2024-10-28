using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Interfaz
{
    public interface IKardexAD
    {
        List<Kardex> RecKardex();
        Kardex? RecKardexXId(int pIdKardex);
        bool InsKardex(Kardex pKardex);
        bool ModKardex(Kardex pKardex);
        bool DelKardex(Kardex pKardex);
    }
}
