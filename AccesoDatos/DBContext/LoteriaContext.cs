using Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.DBContext
{
    public partial class LoteriaContext : DbContext
    {

        private readonly IConfiguration? _configuration;


        public LoteriaContext(IConfiguration configuration)
        {
            _configuration = configuration ;
        }


        public LoteriaContext(DbContextOptions<LoteriaContext> options)
            : base(options)
        {

        }
        

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Kardex> Kardices { get; set; } = null!;
        public virtual DbSet<Parametro> Parametros { get; set; } = null!;
        public virtual DbSet<Sorteo> Sorteos { get; set; } = null!;
        public virtual DbSet<TipoSorteo> TipoSorteos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var connectionString = _configuration?.GetConnectionString("LoteriaBD") ?? throw new InvalidOperationException("Connection string not found");
            optionsBuilder.UseSqlServer(connectionString);

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.ToTable("Cliente");

                entity.Property(e => e.IdCliente)
                    .ValueGeneratedNever()
                    .HasColumnName("idCliente");

                entity.Property(e => e.Bloqueado).HasColumnName("bloqueado");

                entity.Property(e => e.Cedula).HasColumnName("cedula");

                entity.Property(e => e.Clave)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("clave");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FechaBorrado)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaBorrado");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombreUsuario");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("telefono");
            });

            modelBuilder.Entity<Kardex>(entity =>
            {
                entity.HasKey(e => e.IdKardex)
                    .HasName("PK_Historial");

                entity.ToTable("Kardex");

                entity.Property(e => e.IdKardex)
                    .ValueGeneratedNever()
                    .HasColumnName("idKardex");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Monto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("monto");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Numero).HasColumnName("numero");

                entity.Property(e => e.Serie)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("serie");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Kardices)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kardex_Usuario");
            });

            modelBuilder.Entity<Parametro>(entity =>
            {
                entity.HasKey(e => e.IdParametro)
                    .HasName("PK_Parametros_1");

                entity.ToTable("Parametro");

                entity.Property(e => e.IdParametro)
                    .ValueGeneratedNever()
                    .HasColumnName("idParametro");

                entity.Property(e => e.Cedula).HasColumnName("cedula");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IdSorteo).HasColumnName("idSorteo");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.RazonSocial).HasColumnName("razonSocial");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("telefono");
            });

            modelBuilder.Entity<Sorteo>(entity =>
            {
                entity.HasKey(e => e.IdSorteo);

                entity.ToTable("Sorteo");

                entity.Property(e => e.IdSorteo)
                    .ValueGeneratedNever()
                    .HasColumnName("idSorteo");

                entity.Property(e => e.IdTipoSorteo).HasColumnName("idTipoSorteo");

                entity.Property(e => e.Monto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("monto");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Numero).HasColumnName("numero");
            });

            modelBuilder.Entity<TipoSorteo>(entity =>
            {
                entity.HasKey(e => e.IdTipoSorteo);

                entity.ToTable("TipoSorteo");

                entity.Property(e => e.IdTipoSorteo)
                    .ValueGeneratedNever()
                    .HasColumnName("idTipoSorteo");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaFin");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaInicio");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.NumeroGanador).HasColumnName("numeroGanador");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TipoSorteos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipoSorteo_Usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario)
                    .ValueGeneratedNever()
                    .HasColumnName("idUsuario");

                entity.Property(e => e.Clave)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("clave");

                entity.Property(e => e.Correo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombreUsuario");

                entity.Property(e => e.Rol).HasColumnName("rol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
