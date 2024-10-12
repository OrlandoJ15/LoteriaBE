using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class Cliente
    {
        public int IdCliente { get; set; }
        public int Cedula { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaBorrado { get; set; }
        public int Bloqueado { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Clave { get; set; } = null!;
    }
}
