using Microsoft.EntityFrameworkCore;
using v5marketing.Models;

namespace v5marketing.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Nome)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Login)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.SenhaHash)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Perfil)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<AuditoriaLog>()
                .Property(a => a.UsuarioLogin)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<AuditoriaLog>()
                .Property(a => a.Acao)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<AuditoriaLog>()
                .Property(a => a.Entidade)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<AuditoriaLog>()
                .Property(a => a.Descricao)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<AuditoriaLog>()
                .Property(a => a.Ip)
                .HasMaxLength(100);
        }
    }
}