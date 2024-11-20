using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class TipoSorteoGeneral
    {
        public TipoSorteoGeneral()
        {
            TipoSorteos = new HashSet<TipoSorteo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int Fondo { get; set; }
        public int PorcentajePago { get; set; }
        public TimeSpan HoraFin { get; set; }

        public virtual ICollection<TipoSorteo> TipoSorteos { get; set; }
    }
}
