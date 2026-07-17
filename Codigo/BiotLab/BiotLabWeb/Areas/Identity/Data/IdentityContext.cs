using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BiotLabWeb.Areas.Identity.Data
{
    public class IdentityContext : IdentityDbContext<UsuarioIdentity>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        public DbSet<ConviteUsuario> ConvitesUsuarios => Set<ConviteUsuario>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UsuarioIdentity>(entity =>
            {
                entity.ToTable("AspNetUsers");

                entity.Property(u => u.NomeCompleto)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(u => u.TipoUsuario)
                    .HasMaxLength(30);
            });

            builder.Entity<ConviteUsuario>(entity =>
            {
                entity.ToTable("ConvitesUsuarios");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id)
                    .HasColumnType("int unsigned");

                entity.Property(c => c.NomeCompleto)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(c => c.Email)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(c => c.Perfil)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(c => c.Codigo)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(c => c.UsuarioId)
                    .HasMaxLength(450);

                entity.HasIndex(c => c.Codigo)
                    .IsUnique();

                entity.HasIndex(c => new { c.Email, c.AceitoEm });
            });
        }
    }
}
