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
        }
    }
}