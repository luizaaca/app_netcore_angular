using Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.EF.Configuration
{
    internal static class Mappings
    {
        internal static void UsuarioMap(EntityTypeBuilder<Usuario> map)
        {
            map.HasKey(p => p.Id);
            
            map.HasOne(c => c.Condominio).WithOne().HasForeignKey<Condominio>(c => c.Id);
        }

        internal static void AdministradoraMap(EntityTypeBuilder<Administradora> map)
        {
            map.HasKey(p => p.Id);
        }

        internal static void AssuntoMap(EntityTypeBuilder<Assunto> map)
        {
            map.HasKey(p => p.Id);
        }

        internal static void CondominioMap(EntityTypeBuilder<Condominio> map)
        {
            map.HasKey(p => p.Id);
            
            //map.HasOne(u => u.Responsavel).WithOne().HasForeignKey<Usuario>(u => u.Id);
        }
    }
}
