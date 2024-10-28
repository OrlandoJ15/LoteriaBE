using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class Kardex
    {
        public int IdKardex { get; set; }
        public string Serie { get; set; } = null!;
        public int Numero { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Monto { get; set; }
        public int IdUsuario { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; } = null!;
    }
}
