using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class Parametro
    {
        public int Id { get; set; }
        public int RazonSocial { get; set; }
        public string Nombre { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Cedula { get; set; }
        public int IdSorteo { get; set; }
    }
}
