using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Models;

namespace AccesoDatos.Interfaz
{
    public interface IUsuarioAD
    {
             
        List<Usuario> RecUsuario();
        Usuario? RecUsuarioXId(int pIdUsuario);
        bool InsUsuario(Usuario pUsuarioPA);
        bool ModUsuario(Usuario pUsuarioPA);
        bool DelUsuario(Usuario pUsuarioPA);
        Usuario? ValidarLoginUsuario(int pId, string pClave);
        bool ModClaveUsuario(int pId, string pClave);


    }
}
