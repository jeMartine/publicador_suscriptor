using Microsoft.EntityFrameworkCore;
using BancoTurnosApp.src.models;

namespace BancoTurnosApp.src.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Turno> Turnos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== CONFIGURACIÓN DE CLIENTE ==========
            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Cedula)
                .IsUnique();

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Id)
                .IsRequired();

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Cedula)
                .IsRequired()
                .HasMaxLength(20);

            // ========== CONFIGURACIÓN DE SERVICIO ==========
            modelBuilder.Entity<Servicio>()
                .HasKey(s => s.id);

            modelBuilder.Entity<Servicio>()
                .HasIndex(s => s.Codigo)
                .IsUnique();

            modelBuilder.Entity<Servicio>()
                .Property(s => s.id)
                .IsRequired();

            modelBuilder.Entity<Servicio>()
                .Property(s => s.Codigo)
                .IsRequired()
                .HasMaxLength(10);

            modelBuilder.Entity<Servicio>()
                .Property(s => s.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Servicio>()
                .Property(s => s.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            // ========== CONFIGURACIÓN DE TURNO ==========
            modelBuilder.Entity<Turno>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Turno>()
                .HasIndex(t => t.Codigo)
                .IsUnique();

            modelBuilder.Entity<Turno>()
                .Property(t => t.Id)
                .IsRequired();

            modelBuilder.Entity<Turno>()
                .Property(t => t.Codigo)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Turno>()
                .Property(t => t.FechaCreacion)
                .IsRequired();

            modelBuilder.Entity<Turno>()
                .Property(t => t.Estado)
                .IsRequired()
                .HasConversion<string>(); // Guarda el enum como string en la BD

            // Relación Turno -> Cliente (muchos a uno)
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Cliente)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // No eliminar cliente si tiene turnos

            // Relación Turno -> Servicio (muchos a uno)
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Servicio)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // No eliminar servicio si tiene turnos


        }
    }
}