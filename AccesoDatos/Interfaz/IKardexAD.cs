using Entidades.Models;

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
