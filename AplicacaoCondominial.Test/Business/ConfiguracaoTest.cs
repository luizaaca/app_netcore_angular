using Core.Business.DTOs;
using Core.Business.Wraps;
using System;
using System.Linq;
using Xunit;

namespace AplicacaoCondominial.Test.Business
{
    public class ConfiguracaoTest
    {
        Config config;

        public ConfiguracaoTest()
        {
            config = new Config();
        }

        #region .: Condominio :.

        [Fact]
        public async void CondominioNaoCadastraSemNome()
        {
            var administradora = GerarAdministradora();
            var response = await config.ConfiguracaoBusiness
                .SalvarCondominioAsync(new BaseRequest<Condominio>(new Condominio
                {
                    IdAdministradora = administradora.Id
                }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(config.Context.Condominios.Where(c => c.Nome == null).ToList());
        }

        [Fact]
        public async void CondominioNaoCadastraSemAdministradora()
        {
            var response = await config.ConfiguracaoBusiness.SalvarCondominioAsync(
                new BaseRequest<Condominio>(
                    new Condominio
                    {
                        Nome = "Condomínio Teste 1"
                    }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(config.Context.Condominios
                                .Where(c => c.Nome == "Condomínio Teste 1")
                                .ToList());
        }

        [Fact]
        public async void CondominioCadastraComUsuarioInexistente()
        {
            var administradora = GerarAdministradora();

            var response = await config.ConfiguracaoBusiness.SalvarCondominioAsync(
                new BaseRequest<Condominio>(
                    new Condominio
                    {
                        IdAdministradora = administradora.Id,
                        Nome = "Condomínio Teste 2"
                    }));

            Assert.True(response.Success, response.Message);

            var condominios = config.Context.Condominios
                                .Where(c => c.Nome == "Condomínio Teste 2"
                                            && c.IdAdministradora == administradora.Id)
                                .ToList();

            Assert.Single(condominios);
        }

        [Fact]
        public async void CondominioRecuperadoComSucesso()
        {
            var condominio = GerarCondominio();

            var response = await config.ConfiguracaoBusiness
                .BuscarCondominioAsync(new BaseRequest<int>(condominio.Id));

            Assert.True(response.Success, response.Message);
            Assert.NotNull(response.Result);
            Assert.True(response.Result.Id == condominio.Id
                && response.Result.Nome == condominio.Nome
                && response.Result.IdAdministradora == condominio.IdAdministradora
                && response.Result.IdResponsavel == condominio.IdResponsavel,
                response.Message);
        }

        #endregion

        #region .: Usuario :.

        [Fact]
        public async void UsuarioNaoCadastraSemNome()
        {
            var condominio = GerarCondominio();

            var response = await config.ConfiguracaoBusiness
                .SalvarUsuarioAsync(new BaseRequest<Usuario>(new Usuario
                {
                    IdCondominio = condominio.Id,
                    Tipo = new TipoUsuario { Id = 1 }
                }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(config.Context.Usuarios.Where(u => u.Nome == null).ToList());
        }

        [Fact]
        public async void UsuarioNaoCadastraSemCondominio()
        {
            var nome = $"Joao Ferreira {DateTime.Now.Ticks}";

            var response = await config.ConfiguracaoBusiness
                .SalvarUsuarioAsync(new BaseRequest<Usuario>(new Usuario
                {
                    Nome = nome,
                    Tipo = new TipoUsuario { Id = 1 }
                }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(config.Context.Usuarios
                            .Where(u => u.IdCondominio == 0 && u.Nome == nome).ToList());
        }

        [Theory]
        [InlineData(Core.Model.TipoUsuario.Morador - 1)]
        [InlineData(Core.Model.TipoUsuario.Zelador + 1)]
        public async void UsuarioNaoCadastraSemTipoValido(byte tipo)
        {
            var nome = $"Joao Ferreira {DateTime.Now.Ticks}";

            var response = await config.ConfiguracaoBusiness
                .SalvarUsuarioAsync(new BaseRequest<Usuario>(new Usuario
                {
                    Nome = nome,
                    IdCondominio = GerarCondominio().Id,
                    Tipo = new TipoUsuario { Id = tipo }
                }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(config.Context.Usuarios
                .Where(u => u.Tipo < Core.Model.TipoUsuario.Morador
                || u.Tipo > Core.Model.TipoUsuario.Zelador).ToList());
        }

        [Fact]
        public async void UsuarioCadastraCorretamente()
        {
            var condominio = GerarCondominio();

            var nome = $"Joao Ferreira {DateTime.Now.Ticks}";

            var usuario = new Core.Model.Usuario
            {
                IdCondominio = condominio.Id,
                Nome = nome,
                Tipo = Core.Model.TipoUsuario.Sindico
            };

            config.Context.Usuarios.Add(usuario);
            config.Context.SaveChanges();

            Assert.Single(config.Context.Usuarios.Where(u => u.Id == usuario.Id && u.Id > 0));

            var response = await config.ConfiguracaoBusiness
                .BuscarUsuarioAsync(new BaseRequest<int>(usuario.Id));

            Assert.True(response.Success, response.Message);

            Assert.True(response.Result.Id == usuario.Id
                && response.Result.IdCondominio == condominio.Id
                && response.Result.Nome == nome
                && response.Result.Tipo.Id == (byte)Core.Model.TipoUsuario.Sindico);
        }

        #endregion

        private Core.Model.Administradora GerarAdministradora()
        {
            var administradora = new Core.Model.Administradora
            {
                Nome = $"Administradora 1 {DateTime.Now.Ticks}"
            };

            config.Context.Administradoras.Add(administradora);
            config.Context.SaveChanges();

            Assert.Single(config.Context.Administradoras
                    .Where(a => a.Id == administradora.Id)
                    .ToList());

            return administradora;
        }

        private Core.Model.Condominio GerarCondominio()
        {
            var condominio = new Core.Model.Condominio
            {
                IdAdministradora = 35,
                IdResponsavel = 48,
                Nome = $"Condomínio Teste {DateTime.Now.Ticks}"
            };

            config.Context.Condominios.Add(condominio);
            config.Context.SaveChanges();

            Assert.Single(config.Context.Condominios
                    .Where(c => c.Id == condominio.Id)
                    .ToList());

            return condominio;
        }
    }
}
