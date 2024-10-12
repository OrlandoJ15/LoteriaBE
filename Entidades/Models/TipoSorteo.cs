using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class TipoSorteo
    {
        public int IdTipoSorteo { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NumeroGanador { get; set; }
        public int IdUsuario { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; } = null!;
    }
}
