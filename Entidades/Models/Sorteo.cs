using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Models
{
    public partial class Sorteo
    {
        public Sorteo()
        {
            DetalleSorteos = new HashSet<DetalleSorteo>();
        }

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdTipoSorteo { get; set; }

        public virtual TipoSorteo? IdTipoSorteoNavigation { get; set; } = null!;
        public virtual Usuario? IdUsuarioNavigation { get; set; } = null!;
        public virtual ICollection<DetalleSorteo> DetalleSorteos { get; set; }

        // Propiedad manual para el nombre del TipoSorteoGeneral (no mapeada a la BD)
        [NotMapped] // Esto indica que no es una columna de la tabla
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreTipoSorteoGeneral { get; set; } = string.Empty;
        public DateTime FechaTipoSorteo { get; set; }
    }
}
