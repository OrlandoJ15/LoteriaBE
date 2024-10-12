using Entidades.Models;

namespace LogicaNegocio.Interfaz
{
    public interface IUsuarioLN
    {


        List<Usuario> RecUsuario();
        Usuario? RecUsuarioXId(int pIdUsuario);
        bool InsUsuario(Usuario pUsuario);
        bool ModUsuario(Usuario pUsuario);
        bool DelUsuario(Usuario pUsuario);
        Usuario? ValidarLoginUsuario(int pId, string pClave);
        bool ModClaveUsuario(int pId, string pClave);

    }
}
