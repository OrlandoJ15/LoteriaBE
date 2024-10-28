using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class Sorteo
    {
        public int IdSorteo { get; set; }
        public string Nombre { get; set; } = null!;
        public int Numero { get; set; }
        public int Monto { get; set; }
        public int IdUsuario { get; set; }
        public int IdTipoSorteo { get; set; }

        public virtual TipoSorteo? IdTipoSorteoNavigation { get; set; } = null!;
        public virtual Usuario? IdUsuarioNavigation { get; set; } = null!;
    }
}
