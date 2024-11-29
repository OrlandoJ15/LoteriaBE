using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class DetalleSorteo
    {
        public int Id { get; set; }
        public int IdSorteo { get; set; }
        public int Numero { get; set; }
        public int Monto { get; set; }

        public virtual Sorteo IdSorteoNavigation { get; set; } = null!;
    }
}
