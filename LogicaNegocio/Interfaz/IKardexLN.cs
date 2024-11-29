using Entidades.Models;

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
