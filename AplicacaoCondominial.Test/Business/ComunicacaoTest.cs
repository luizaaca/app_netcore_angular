using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AplicacaoCondominial.Test.Business
{
    public class ComunicacaoTest
    {
        [Fact]
        public async void MensagemAdministrativaEnviadaCorretamente()
        {
            var administradora = GerarAdministradora();
            Salvar(administradora);
            Assert.True(administradora.Id > 0);

            var condominio = GerarCondominio(administradora.Id);
            Salvar(condominio);
            Assert.True(condominio.Id > 0);

            var usuarioAdministradora = GerarUsuario(condominio.Id, Core.Model.TipoUsuario.Administradora);
            Salvar(usuarioAdministradora);
            Assert.True(usuarioAdministradora.Id > 0);

            var usuarioMorador = GerarUsuario(condominio.Id, Core.Model.TipoUsuario.Morador);
            Salvar(usuarioMorador);
            Assert.True(usuarioMorador.Id > 0);

            var mensagem = $"Mensagem usuario {usuarioMorador.Id} para usuario {usuarioAdministradora.Id}.";

            var response = await Init.ComunicacaoBusiness
                .EnviarComunicadoAsync(new Core.Business.Wraps.BaseRequest<Core.Business.DTOs.Comunicado>(
                    new Core.Business.DTOs.Comunicado {
                        Assunto = Core.Model.Assunto.Administrativo.ToString(),
                        IdUsuario = usuarioMorador.Id,
                        Mensagem = mensagem
                    }));

            Assert.True(response.Success, response.Message);
            //Verificar mensagem
        }

        private void Salvar<T>(T objeto) where T : class
        {
            Init.Context.Set<T>().Add(objeto);
            Init.Context.SaveChanges();
        }

        [Fact]
        public async void MensagemCondominialEnviadaCorretamente()
        {
            var administradora = GerarAdministradora();
            var condominio = GerarCondominio(administradora.Id);
            var usuarioSindico = GerarUsuario(condominio.Id, Core.Model.TipoUsuario.Sindico);
            var usuarioMorador = GerarUsuario(condominio.Id, Core.Model.TipoUsuario.Morador);
        }

        private static Core.Model.Usuario GerarUsuario(int idCondominio, Core.Model.TipoUsuario tipoUsuario)
        {
            var usuario = new Core.Model.Usuario
            {
                IdCondominio = idCondominio,
                Tipo = tipoUsuario,
                Nome = $"Usuario Teste {DateTime.Now.Ticks}"
            };

            Init.Context.Usuarios.Add(usuario);
            Init.Context.SaveChanges();

            return usuario;
        }

        private static Core.Model.Condominio GerarCondominio(int idAdministradora)
        {
            var condominio = new Core.Model.Condominio
            {
                IdAdministradora = idAdministradora,
                Nome = $"Condomínio Teste {DateTime.Now.Ticks}"
            };

            Init.Context.Condominios.Add(condominio);
            Init.Context.SaveChanges();

            Assert.Single(Init.Context.Condominios
                    .Where(c => c.Id == condominio.Id)
                    .ToList());

            return condominio;
        }

        private static Core.Model.Administradora GerarAdministradora()
        {
            var administradora = new Core.Model.Administradora
            {
                Nome = "Administradora Comunicacao"
            };

            Init.Context.Administradoras.Add(administradora);
            Init.Context.SaveChanges();

            Assert.Single(Init.Context.Administradoras
                    .Where(a => a.Id == administradora.Id)
                    .ToList());

            return administradora;
        }
    }
}
