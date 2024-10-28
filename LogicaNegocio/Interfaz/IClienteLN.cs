using Entidades.Models;

namespace LogicaNegocio.Interfaz
{
    public interface IClienteLN
    {


        List<Cliente> RecCliente();
        Cliente? RecClienteXId(int pIdCliente);
        bool InsCliente(Cliente pCliente);
        bool ModCliente(Cliente pCliente);
        bool DelCliente(Cliente pCliente);
        Cliente? ValidarLoginCliente(int pId, string pClave);
        bool ModClaveCliente(int pId, string pClave);

    }
}
