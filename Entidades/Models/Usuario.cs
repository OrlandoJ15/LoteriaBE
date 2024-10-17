using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Kardices = new HashSet<Kardex>();
            TipoSorteos = new HashSet<TipoSorteo>();
        }

        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public int Rol { get; set; }
        public string Correo { get; set; } = null!;
        public string? Clave { get; set; }

        public virtual ICollection<Kardex> Kardices { get; set; }
        public virtual ICollection<TipoSorteo> TipoSorteos { get; set; }

    }
}
