using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AccesoDatos;
using Microsoft.Extensions.Configuration;
using Entidades.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AccesoDatos.DBContext
{
    public partial class LoteriaContext : DbContext
    {
        private readonly IConfiguration? _configuration;


        public LoteriaContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public LoteriaContext(DbContextOptions<LoteriaContext> options)
            : base(options)
        {

        }


        public virtual DbSet<DetalleSorteo> DetalleSorteos { get; set; } = null!;
        public virtual DbSet<Kardex> Kardices { get; set; } = null!;
        public virtual DbSet<Parametro> Parametros { get; set; } = null!;
        public virtual DbSet<Sorteo> Sorteos { get; set; } = null!;
        public virtual DbSet<Suite> Suites { get; set; } = null!;
        public virtual DbSet<TipoSorteo> TipoSorteos { get; set; } = null!;
        public virtual DbSet<TipoSorteoGeneral> TipoSorteoGenerals { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var connectionString = _configuration?.GetConnectionString("LoteriaBD") ?? throw new InvalidOperationException("Connection string not found");
            optionsBuilder.UseSqlServer(connectionString);

        }







        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetalleSorteo>(entity =>
            {
                entity.ToTable("DetalleSorteo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdSorteo).HasColumnName("idSorteo");

                entity.Property(e => e.Monto).HasColumnName("monto");

                entity.Property(e => e.Numero).HasColumnName("numero");

                entity.HasOne(d => d.IdSorteoNavigation)
                    .WithMany(p => p.DetalleSorteos)
                    .HasForeignKey(d => d.IdSorteo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetalleSorteo_Sorteo");
            });

            modelBuilder.Entity<Kardex>(entity =>
            {
                entity.ToTable("Kardex");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdSorteo).HasColumnName("idSorteo");

                entity.Property(e => e.Id).HasColumnName("id");

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
                entity.ToTable("Parametro");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

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
                entity.ToTable("Sorteo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdTipoSorteo).HasColumnName("idTipoSorteo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdTipoSorteoNavigation)
                    .WithMany(p => p.Sorteos)
                    .HasForeignKey(d => d.IdTipoSorteo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sorteo_TipoSorteo1");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Sorteos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sorteo_Usuario");
            });

            modelBuilder.Entity<Suite>(entity =>
            {
                entity.ToTable("Suite");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ImagenUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("imagenUrl");
            });

            modelBuilder.Entity<TipoSorteo>(entity =>
            {
                entity.ToTable("TipoSorteo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fecha)
                    .HasColumnType("date")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdTipoSorteoGeneral).HasColumnName("idTipoSorteoGeneral");

                entity.Property(e => e.NumeroGanador).HasColumnName("numeroGanador");

                //Propiedad anadida
                entity.Property(e => e.NombreTipoSorteoGeneral).HasColumnName("nombreTipoSorteoGeneral");

                entity.HasOne(d => d.IdTipoSorteoGeneralNavigation)
                    .WithMany(p => p.TipoSorteos)
                    .HasForeignKey(d => d.IdTipoSorteoGeneral)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipoSorteo_TipoSorteoGeneral");
            });

            modelBuilder.Entity<TipoSorteoGeneral>(entity =>
            {
                entity.ToTable("TipoSorteoGeneral");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fondo).HasColumnName("fondo");

                entity.Property(e => e.HoraFin).HasColumnName("horaFin");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.PorcentajePago).HasColumnName("porcentajePago");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

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
