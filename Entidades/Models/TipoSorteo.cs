using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class TipoSorteo
    {
        public TipoSorteo()
        {
            Sorteos = new HashSet<Sorteo>();
        }

        public int IdTipoSorteo { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NumeroGanador { get; set; }

        public virtual ICollection<Sorteo> Sorteos { get; set; }
    }
}
