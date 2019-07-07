using Core.Business;
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.EF.Configuration;
using Repository.EF.Repository;

namespace AplicacaoCondominial.Test
{
    public class Config
    {
        public readonly EFContext Context;
        public readonly IConfiguracaoBusiness ConfiguracaoBusiness;
        public readonly IComunicacaoBusiness ComunicacaoBusiness;

        public Config()
        {
            var services = new ServiceCollection();
            services.AddDbContext<EFContext>(opt => opt.UseInMemoryDatabase("TestDB"), ServiceLifetime.Singleton);

            services.AddTransient<IAdministradoraRepository, AdministradoraRepository>();
            services.AddTransient<ICondominioRepository, CondominioRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();

            services.AddTransient<IConfiguracaoBusiness, ConfiguracaoBusiness>();
            services.AddTransient<IComunicacaoBusiness, ComunicacaoBusiness>();

            var serviceProvider = services.BuildServiceProvider();

            Context = serviceProvider.GetService<EFContext>();
            ConfiguracaoBusiness = serviceProvider.GetService<IConfiguracaoBusiness>();
            ComunicacaoBusiness = serviceProvider.GetService<IComunicacaoBusiness>();
        }
    }
}
