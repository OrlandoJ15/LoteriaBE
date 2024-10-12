using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class Sorteo
    {
        public int IdSorteo { get; set; }
        public string Nombre { get; set; } = null!;
        public int Numero { get; set; }
        public decimal Monto { get; set; }
        public int IdTipoSorteo { get; set; }
    }
}
