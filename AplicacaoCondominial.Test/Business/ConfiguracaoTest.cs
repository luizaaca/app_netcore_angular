using Core.Business.DTOs;
using Core.Business.Wraps;
using System;
using System.Linq;
using Xunit;

namespace AplicacaoCondominial.Test.Business
{
    public class ConfiguracaoTest
    {
        #region .: Condominio :.

        [Fact]
        public async void CondominioNaoCadastraSemNome()
        {
            var administradora = GerarAdministradora();
            var response = await Init.ConfiguracaoBusiness
                .SalvarCondominioAsync(new BaseRequest<Condominio>(new Condominio
                {
                    IdAdministradora = administradora.Id
                }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(Init.Context.Condominios.Where(c => c.Nome == null).ToList());
        }

        [Fact]
        public async void CondominioNaoCadastraSemAdministradora()
        {
            var response = await Init.ConfiguracaoBusiness.SalvarCondominioAsync(
                new BaseRequest<Condominio>(
                    new Condominio
                    {
                        Nome = "Condomínio Teste 1"
                    }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(Init.Context.Condominios
                                .Where(c => c.Nome == "Condomínio Teste 1")
                                .ToList());
        }

        [Fact]
        public async void CondominioCadastraComUsuarioInexistente()
        {
            var administradora = GerarAdministradora();

            var response = await Init.ConfiguracaoBusiness.SalvarCondominioAsync(
                new BaseRequest<Condominio>(
                    new Condominio
                    {
                        IdAdministradora = administradora.Id,
                        Nome = "Condomínio Teste 2"
                    }));

            Assert.True(response.Success, response.Message);

            var condominios = Init.Context.Condominios
                                .Where(c => c.Nome == "Condomínio Teste 2"
                                            && c.IdAdministradora == administradora.Id)
                                .ToList();

            Assert.Single(condominios);
        }

        [Fact]
        public async void CondominioRecuperadoComSucesso()
        {
            var condominio = GerarCondominio();

            var response = await Init.ConfiguracaoBusiness
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

            var response = await Init.ConfiguracaoBusiness
                .SalvarUsuarioAsync(new BaseRequest<Usuario>(new Usuario
                {
                    IdCondominio = condominio.Id,
                    Tipo = new TipoUsuario { Id = 1 }
                }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(Init.Context.Usuarios.Where(u => u.Nome == null).ToList());
        }

        [Fact]
        public async void UsuarioNaoCadastraSemCondominio()
        {
            var nome = $"Joao Ferreira {DateTime.Now.Ticks}";

            var response = await Init.ConfiguracaoBusiness
                .SalvarUsuarioAsync(new BaseRequest<Usuario>(new Usuario
                {
                    Nome = nome,
                    Tipo = new TipoUsuario { Id = 1 }
                }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(Init.Context.Usuarios
                            .Where(u => u.IdCondominio == 0 && u.Nome == nome).ToList());
        }

        [Theory]
        [InlineData(Core.Model.TipoUsuario.Morador - 1)]
        [InlineData(Core.Model.TipoUsuario.Zelador + 1)]
        public async void UsuarioNaoCadastraSemTipoValido(byte tipo)
        {
            var nome = $"Joao Ferreira {DateTime.Now.Ticks}";

            var response = await Init.ConfiguracaoBusiness
                .SalvarUsuarioAsync(new BaseRequest<Usuario>(new Usuario
                {
                    Nome = nome,
                    IdCondominio = GerarCondominio().Id,
                    Tipo = new TipoUsuario { Id = tipo }
                }));

            Assert.False(response.Success, "O método deveria retornar sem sucesso.");
            Assert.Empty(Init.Context.Usuarios
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

            Init.Context.Usuarios.Add(usuario);
            Init.Context.SaveChanges();

            Assert.Single(Init.Context.Usuarios.Where(u => u.Id == usuario.Id && u.Id > 0));

            var response = await Init.ConfiguracaoBusiness
                .BuscarUsuarioAsync(new BaseRequest<int>(usuario.Id));

            Assert.True(response.Success, response.Message);

            Assert.True(response.Result.Id == usuario.Id
                && response.Result.IdCondominio == condominio.Id
                && response.Result.Nome == nome
                && response.Result.Tipo.Id == (byte)Core.Model.TipoUsuario.Sindico);
        }
                
        #endregion

        private static Core.Model.Administradora GerarAdministradora()
        {
            var administradora = new Core.Model.Administradora
            {
                Nome = $"Administradora 1 {DateTime.Now.Ticks}"
            };

            Init.Context.Administradoras.Add(administradora);
            Init.Context.SaveChanges();

            Assert.Single(Init.Context.Administradoras
                    .Where(a => a.Id == administradora.Id)
                    .ToList());

            return administradora;
        }

        private static Core.Model.Condominio GerarCondominio()
        {
            var condominio = new Core.Model.Condominio
            {
                IdAdministradora = 35,
                IdResponsavel = 48,
                Nome = $"Condomínio Teste {DateTime.Now.Ticks}"
            };

            Init.Context.Condominios.Add(condominio);
            Init.Context.SaveChanges();

            Assert.Single(Init.Context.Condominios
                    .Where(c => c.Id == condominio.Id)
                    .ToList());

            return condominio;
        }
    }
}
