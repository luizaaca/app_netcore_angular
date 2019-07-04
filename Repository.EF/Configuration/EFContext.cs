using Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository.EF.Configuration
{
    public class EFContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Administradora> Administradoras { get; set; }
        public DbSet<Condominio> Condominios { get; set; }
        public DbSet<Assunto> Assuntos { get; set; }

        public EFContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Mappings.UsuarioMap(modelBuilder.Entity<Usuario>());
            Mappings.AdministradoraMap(modelBuilder.Entity<Administradora>());
            Mappings.CondominioMap(modelBuilder.Entity<Condominio>());
            Mappings.AssuntoMap(modelBuilder.Entity<Assunto>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
