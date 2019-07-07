using Core.Business;
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.EF.Configuration;
using Repository.EF.Repository;

namespace AplicacaoCondominial.Test
{
    public static class Init
    {
        public static readonly EFContext Context;
        public static readonly IConfiguracaoBusiness ConfiguracaoBusiness;
        public static readonly IComunicacaoBusiness ComunicacaoBusiness;

        static Init()
        {
            var services = new ServiceCollection();
            services.AddDbContext<EFContext>(opt => opt.UseInMemoryDatabase("TestDB"));

            services.AddTransient<IAdministradoraRepository, AdministradoraRepository>();
            services.AddTransient<ICondominioRepository, CondominioRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();

            services.AddTransient<IConfiguracaoBusiness, ConfiguracaoBusiness>();
            services.AddTransient<IComunicacaoBusiness, ComunicacaoBusiness>();

            var serviceProvider = services.BuildServiceProvider();

            Context = serviceProvider.GetService<EFContext>();
            ConfiguracaoBusiness = serviceProvider.GetService<IConfiguracaoBusiness>();
        }
    }
}
