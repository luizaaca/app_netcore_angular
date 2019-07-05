using Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.EF.Configuration;
using System;
using System.Linq;

namespace AplicacaoCondominial
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetService<EFContext>())
            {
                if (context.Administradoras.Any())
                    return;

                context.Administradoras.AddRange(
                    new Administradora
                    {
                        Nome = "Administradora 1"
                    },
                    new Administradora
                    {
                        Nome = "Administradora 2"
                    },
                    new Administradora
                    {
                        Nome = "Administradora 3"
                    });               

                context.SaveChanges();
            }
        }
    }
}
