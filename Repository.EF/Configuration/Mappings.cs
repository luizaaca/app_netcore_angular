using Core.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EF.Configuration
{
    internal static class Mappings
    {
        internal static void UsuarioMap(EntityTypeBuilder<Usuario> map)
        {
            map.HasKey(p => p.Id);
            
            map.HasOne(c => c.Condominio)
                .WithMany()
                .HasForeignKey(u => u.IdCondominio);
        }

        internal static void AdministradoraMap(EntityTypeBuilder<Administradora> map)
        {
            map.HasKey(p => p.Id);
        }        

        internal static void CondominioMap(EntityTypeBuilder<Condominio> map)
        {
            map.HasKey(p => p.Id);
            
            //map.HasOne(u => u.Responsavel)
            //    .WithOne()
            //    .HasForeignKey<Condominio>(c => c.IdResponsavel);

            map.HasOne(u => u.Administradora)
                .WithOne()
                .HasForeignKey<Condominio>(c => c.IdAdministradora);
        }
    }
}
