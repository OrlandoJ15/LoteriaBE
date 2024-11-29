using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Models
{
    public partial class TipoSorteo
    {
        public TipoSorteo()
        {
            Sorteos = new HashSet<Sorteo>();
        }

        public int Id { get; set; }
        public int? NumeroGanador { get; set; }
        public int IdTipoSorteoGeneral { get; set; }
        public DateTime Fecha { get; set; }
        

        public virtual TipoSorteoGeneral IdTipoSorteoGeneralNavigation { get; set; } = null!;
        public virtual ICollection<Sorteo> Sorteos { get; set; }


        // Propiedad manual para el nombre del TipoSorteoGeneral (no mapeada a la BD)
        [NotMapped] // Esto indica que no es una columna de la tabla
        public string? NombreTipoSorteoGeneral { get; set; } = string.Empty;
    }
}
