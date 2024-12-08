using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class TipoSorteoGeneralExtraordinario
    {
        public TipoSorteoGeneralExtraordinario()
        {
            TipoSorteosGenerales = new HashSet<TipoSorteoGeneral>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int Fondo { get; set; }
        public int PorcentajePago { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }


        public virtual ICollection<TipoSorteoGeneral> TipoSorteosGenerales { get; set; }
    }
}
