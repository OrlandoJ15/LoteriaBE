using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public int IdTipoSorteoGeneralExtraordinario { get; set; }


        public virtual TipoSorteoGeneralExtraordinario IdTipoSorteoGeneralExtraordinarioNavigation { get; set; } = null!;



        public virtual ICollection<TipoSorteo> TipoSorteos { get; set; }

        // Propiedad manual para el nombre del TipoSorteoGeneral (no mapeada a la BD)
        [NotMapped] // Esto indica que no es una columna de la tabla
        public string? NombreTipoSorteoGeneralExtraordinario { get; set; } = string.Empty;

    }




}
